using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IInteractable
{
    [Header("NPC")]
    [Tooltip("Name of NPC")]
    [SerializeField] private string characterName;
    [Tooltip("Game object where the player will talk to")]
    [SerializeField] private GameObject head;
    private DialogueController dialogueController;
    private DialogueManager dialogueManager;
    private Dictionary<string, bool> dialogueOptions;
    private Dictionary<string, string> dialogueResponses;
    private GameObject mainPlayer;

    private PlayerController playerController;
    private NPCBaseState currentState;
    private NPCStateFactory states;

    private bool enterTalkState = false;
    private bool readyToTalk = false;

    public string CharacterName { get => characterName; }
    public DialogueController DialogueController { get => dialogueController; }
    public DialogueManager DialogueManager { get => dialogueManager; }
    public Dictionary<string, bool> DialogueOptions { get => dialogueOptions; set => dialogueOptions = value; }
    public Dictionary<string, string> DialogueResponses { get => dialogueResponses; set => dialogueResponses = value; }
    public GameObject MainPlayer { get => mainPlayer; }
    public PlayerController PlayerController { get => playerController; }
    public NPCBaseState CurrentState { get => currentState; set => currentState = value; }
    public bool EnterTalkState { get => enterTalkState; set => enterTalkState = value; }
    public bool ReadyToTalk { get => readyToTalk; set => readyToTalk = value; }


    private void Start()
    {
        mainPlayer = GameObject.Find("Player");
        playerController = mainPlayer.GetComponent<PlayerController>();

        states = new NPCStateFactory(this);
        currentState = states.NPCRoamingState();
        dialogueController = GetComponent<DialogueController>();
        dialogueController.Setup(playerController.QuestManager, playerController.InventoryManager, playerController);
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        currentState.EnterState();
    }

    public void Interact()
    {
        playerController.TargetPosition = head.transform.position;
        playerController.SelectedNPC = this;
        playerController.IsTalkState = true;
        enterTalkState = true;
    }

    public string Message()
    {
        return "Talk to " + characterName;
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void Deselect()
    { }
}
