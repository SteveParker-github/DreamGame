using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    [Tooltip("walking Speed of Player")]
    [SerializeField]
    private float WALKSPEED = 4.0f;
    [Tooltip("Rotation speed of camera pitch")]
    [SerializeField]
    private float VERTICALSPEED = 1.0f;
    [Tooltip("Rotation speed of player")]
    [SerializeField]
    private float HORIZONTALSPEED = 1.0f;

    private Controls controls;
    private CharacterController characterController;

    private GameObject mainCamera;
    private float cameraTargetPitch;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float moveSpeed;

    void Awake()
    {
        controls = new Controls();
        characterController = GetComponent<CharacterController>();

        // get a reference to our main camera
		if (mainCamera == null)
		{
			mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}

        controls.Player.PlayerMovement.started += OnMovementInput;
        controls.Player.PlayerMovement.canceled += OnMovementInput;
        controls.Player.PlayerMovement.performed += OnMovementInput;
        controls.Player.PlayerLook.started += OnLookInput;
        controls.Player.PlayerLook.canceled += OnLookInput;
        controls.Player.PlayerLook.performed += OnLookInput;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if (lookInput != Vector2.zero)
        {
            RotatePlayer();            
        }
    }

    private void MovePlayer()
    {
        float targetSpeed = WALKSPEED;

        if (moveInput == Vector2.zero) targetSpeed = 0.0f;

        moveSpeed = targetSpeed * moveInput.magnitude;

        Vector3 inputDirection = new Vector3(moveInput.x, 0.0f, moveInput.y).normalized;

        if (moveInput != Vector2.zero)
        {
            inputDirection = transform.right * moveInput.x + transform.forward * moveInput.y;
        }

        characterController.Move(inputDirection.normalized * (moveSpeed * Time.deltaTime));
    }

    private void RotatePlayer()
    {
		cameraTargetPitch += lookInput.y * VERTICALSPEED;

		cameraTargetPitch = ClampAngle(cameraTargetPitch);

        mainCamera.transform.localRotation = Quaternion.Euler(cameraTargetPitch, 0.0f, 0.0f);

        transform.Rotate(Vector3.up * lookInput.x * HORIZONTALSPEED);
    }

	// clamp our pitch rotation
    private float ClampAngle(float lfAngle)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, -90.0f, 90.0f);
	}

    private void OnMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

        private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
