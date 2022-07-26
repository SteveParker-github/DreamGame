using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("UI")]
    [Tooltip("GameObject for info UI")]
    [SerializeField] private GameObject infoGameObject;
    private GameManager gameManager;
    private TextMeshProUGUI infoText;
    private UIManager uIManager;
    private CharacterController characterController;
    private PlayerBaseState currentState;
    private PlayerStateFactory states;
    private GameObject mainCamera;
    [SerializeField] private AudioSource footStepSource;
    public AudioSource FootStepSource { get => footStepSource; }
    [SerializeField] private AudioSource ambientSound;
    public AudioSource AmbientSound { get => ambientSound; }

    #region Animation
    private Animator animator;
    private readonly int movementXHash = Animator.StringToHash("MovementX");
    private readonly int movementYHash = Animator.StringToHash("MovementY");
    public Animator Animator { get => animator; set => animator = value; }
    public int MovementXHash { get => movementXHash; }
    public int MovementYHash { get => movementYHash; }
    #endregion

    #region controls
    private Controls controls;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isInteractInput;
    private bool isShootInput;
    private bool isAbsorbInput;
    private bool isInventoryMenuInput;
    private bool isQuickSave;
    private bool isQuickLoad;
    private bool isMainMenuInput;

    public Vector2 MoveInput { get => moveInput; }
    public Vector2 LookInput { get => lookInput; }
    public bool IsInteractInput { get => isInteractInput; set => isInteractInput = value; }
    public bool IsShootInput { get => isShootInput; set => isShootInput = value; }
    public bool IsAbsorbInput { get => isAbsorbInput; set => isAbsorbInput = value; }
    public bool IsInventoryMenuInput { get => isInventoryMenuInput; set => isInventoryMenuInput = value; }
    public bool IsQuickSave { get => isQuickSave; set => isQuickSave = value; }
    public bool IsQuickLoad { get => isQuickLoad; set => isQuickLoad = value; }
    public bool IsMainMenuInput { get => isMainMenuInput; set => isMainMenuInput = value; }
    #endregion

    private bool isTalkState;
    private Vector3 targetPosition;
    private NPCController selectedNPC;
    private int optionLocation;
    private string message;
    private bool isPaused;
    private bool readyToTalk;

    private InventoryManager inventoryManager;
    private QuestManager questManager;
    private DreamCatcher dreamCatcher;

    private float suspicionHealth = 0;
    private float damageHealth = 0;
    private int dialogueFails = 0;
    private float maxSuspicionHealth = 400;
    private float recoveryCooldown;

    public TextMeshProUGUI InfoText { get => infoText; }
    public GameManager GameManager { get => gameManager; }
    public UIManager UIManager { get => uIManager; }
    public CharacterController CharacterController { get => characterController; }
    public PlayerBaseState CurrentState { get => currentState; set => currentState = value; }
    public GameObject MainCamera { get => mainCamera; }
    public bool IsTalkState { get => isTalkState; set => isTalkState = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    public NPCController SelectedNPC { get => selectedNPC; set => selectedNPC = value; }
    public int OptionLocation { get => optionLocation; set => optionLocation = value; }
    public string Message { get => message; set => message = value; }
    public bool IsPaused { get => isPaused; set => isPaused = value; }
    public bool ReadyToTalk { get => readyToTalk; set => readyToTalk = value; }
    public InventoryManager InventoryManager { get => inventoryManager; }
    public QuestManager QuestManager { get => questManager; }
    public DreamCatcher DreamCatcher { get => dreamCatcher; }
    public float SuspicionHealth { get => suspicionHealth; }
    public float DamageHealth { get => damageHealth; set => damageHealth = value; }
    public int DialogueFails { get => dialogueFails; set => dialogueFails = value; }

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        questManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        uIManager = GameObject.Find("HUD").GetComponent<UIManager>();
        inventoryManager = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();

        // get a reference to our main camera
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("Player Camera");
        }

        infoText = infoGameObject.GetComponent<TextMeshProUGUI>();

        states = new PlayerStateFactory(this);
        currentState = states.PlayerMainMenuState();
        dreamCatcher = GameObject.FindObjectOfType<DreamCatcher>();
        SetupControls();
    }



    // Start is called before the first frame update
    void Start()
    {
        // currentState.EnterState();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();

        if (isPaused) return;

        if (recoveryCooldown > 0.0f)
        {
            recoveryCooldown -= Time.deltaTime;
        }
        else
        {
            float reduceDamageHealth = (Time.deltaTime * 2.0f);
            damageHealth -= Mathf.Min(damageHealth, reduceDamageHealth);
        }

        suspicionHealth = damageHealth + dialogueFails * 100;

        if (suspicionHealth >= maxSuspicionHealth)
        {
            string sceneName = "";
            currentState.ExitState();
            currentState = states.PlayerWalkingState();
            currentState.EnterState();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                sceneName = SceneManager.GetSceneAt(i).name;

                if (sceneName != "GlobalScene" && sceneName != "MainMenuScene")
                {
                    SceneManager.UnloadSceneAsync(sceneName);
                }
            }

            gameManager.TravelBack(sceneName);
            dreamCatcher.ResetCrystals();
            isTalkState = false;

            dialogueFails = 0;
            damageHealth = 0;
            suspicionHealth = 0;
            return;
        }
    }

    public void Hit(float damage)
    {
        damageHealth += damage;
        suspicionHealth = damageHealth + dialogueFails * 100;

        recoveryCooldown = 5.0f;
    }

    public void ResetSuspicion()
    {
        dialogueFails = 0;
        damageHealth = 0;
    }

    #region  Controller
    private void SetupControls()
    {
        controls = new Controls();

        controls.Player.PlayerMovement.started += OnMovementInput;
        controls.Player.PlayerMovement.canceled += OnMovementInput;
        controls.Player.PlayerMovement.performed += OnMovementInput;
        controls.Player.PlayerLook.started += OnLookInput;
        controls.Player.PlayerLook.canceled += OnLookInput;
        controls.Player.PlayerLook.performed += OnLookInput;
        controls.Player.Interact.started += OnInteractButton;
        controls.Player.Interact.canceled += OnInteractButton;
        controls.Player.Shoot.started += OnShootButton;
        controls.Player.Shoot.canceled += OnShootButton;
        controls.Player.Absorb.started += OnAbsorbButtton;
        controls.Player.Absorb.canceled += OnAbsorbButtton;
        controls.Player.InventoryMenu.started += OnInventoryMenuButton;
        controls.Player.InventoryMenu.canceled += OnInventoryMenuButton;
        controls.Player.QuickSave.started += OnQuickSaveButton;
        controls.Player.QuickSave.canceled += OnQuickSaveButton;
        controls.Player.QuickLoad.started += OnQuickLoadButton;
        controls.Player.QuickLoad.canceled += OnQuickLoadButton;
        controls.Player.MainMenu.started += OnMainMenuButton;
        controls.Player.MainMenu.canceled += OnMainMenuButton;
    }

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void OnInteractButton(InputAction.CallbackContext context)
    {
        isInteractInput = context.ReadValueAsButton();
    }

    private void OnShootButton(InputAction.CallbackContext context)
    {
        isShootInput = context.ReadValueAsButton();
    }

    private void OnAbsorbButtton(InputAction.CallbackContext context)
    {
        isAbsorbInput = context.ReadValueAsButton();
    }

    private void OnInventoryMenuButton(InputAction.CallbackContext context)
    {
        isInventoryMenuInput = context.ReadValueAsButton();
    }

    private void OnQuickSaveButton(InputAction.CallbackContext context)
    {
        isQuickSave = context.ReadValueAsButton();
    }

    private void OnQuickLoadButton(InputAction.CallbackContext context)
    {
        isQuickLoad = context.ReadValueAsButton();
    }

    private void OnMainMenuButton(InputAction.CallbackContext context)
    {
        isMainMenuInput = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    #endregion
}
