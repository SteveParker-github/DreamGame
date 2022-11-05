using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkingState : PlayerBaseState
{
    public PlayerWalkingState(PlayerController playerController, PlayerStateFactory playerStateFactory)
    : base(playerController, playerStateFactory)
    { }

    [Header("Player")]
    [Tooltip("walking Speed of Player")]
    [SerializeField]
    private const float WALKSPEED = 5.0f;
    [Tooltip("Speed of looking around")]
    [SerializeField]
    private const float SENSITIVITY = 1.0f;
    private const float SHOOTINGMAXCOOLDOWN = 1.0f;
    private const float SAVINGMAXCOOLDOWN = 5.0f;
    private const float FIRINGMAXCOOLDOWN = 0.5f;
    private const int MAXPROJECTILES = 4;

    private float moveSpeed;
    private float cameraTargetPitch;
    private bool isShooting;
    private bool isAbsorbing;
    private float shootingCooldown;
    private float savingCooldown;
    private IInteractable currentSelection;

    public override void EnterState()
    {
        //lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        ctx.UIManager.ToggleCrosshair();
        isShooting = false;
        isAbsorbing = false;
        shootingCooldown = 0;
        savingCooldown = 0;
        ctx.IsMainMenuInput = false;
        ctx.IsInventoryMenuInput = false;
    }

    public override void UpdateState()
    {
        if (ctx.IsPaused) return;
        
        Save();
        Load();
        MovePlayer();
        RotatePlayer();
        DetectInteractable();
        Combat();

        CheckSwitchState();
    }

    public override void ExitState()
    {
        ctx.FootStepSource.Stop();
        ctx.Animator.SetFloat(ctx.MovementXHash, 0);
        ctx.Animator.SetFloat(ctx.MovementYHash, 0);
        Cursor.lockState = CursorLockMode.Confined;
        ctx.UIManager.ToggleCrosshair();
    }

    public override void CheckSwitchState()
    {
        if (ctx.IsInventoryMenuInput)
        {
            ctx.IsInventoryMenuInput = false;
            SwitchState(factory.PlayerMenuState());
            return;
        }

        if (ctx.IsTalkState)
        {
            SwitchState(factory.PlayerPrepareTalkState());
            return;
        }

        if (ctx.IsMainMenuInput)
        {
            ctx.IsMainMenuInput = false;
            SwitchState(factory.PlayerMainMenuState());
            return;
        }
    }

    private void MovePlayer()
    {
        float targetSpeed = WALKSPEED;

        if (ctx.MoveInput == Vector2.zero) targetSpeed = 0.0f;

        moveSpeed = targetSpeed * ctx.MoveInput.magnitude;

        Vector3 inputDirection = new Vector3(ctx.MoveInput.x, 0.0f, ctx.MoveInput.y).normalized;

        if (ctx.MoveInput != Vector2.zero)
        {
            inputDirection = ctx.transform.right * ctx.MoveInput.x + ctx.transform.forward * ctx.MoveInput.y;
            if (!ctx.FootStepSource.isPlaying)
            {
                ctx.FootStepSource.Play();
            }
        }
        else
        {
            if (ctx.FootStepSource.isPlaying)
            {
                ctx.FootStepSource.Stop();
            }
        }

        ctx.Animator.SetFloat(ctx.MovementXHash, ctx.MoveInput.x);
        ctx.Animator.SetFloat(ctx.MovementYHash, ctx.MoveInput.y);
        ctx.CharacterController.Move(inputDirection.normalized * (moveSpeed * Time.deltaTime) + new Vector3(0.0f, -9.81f * Time.deltaTime, 0.0f));
    }

    private void RotatePlayer()
    {
        cameraTargetPitch += ctx.LookInput.y * SENSITIVITY;

        cameraTargetPitch = ClampAngle(cameraTargetPitch);

        ctx.MainCamera.transform.localRotation = Quaternion.Euler(cameraTargetPitch, 0.0f, 0.0f);

        ctx.transform.Rotate(Vector3.up * ctx.LookInput.x * SENSITIVITY);
    }

    // clamp our pitch rotation
    private float ClampAngle(float lfAngle)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, -80.0f, 80.0f);
    }

    private void DetectInteractable()
    {
        //bit shift layer 7 (Interactable)
        int layerMask = 1 << 7;

        RaycastHit hit;

        if (Physics.Raycast(ctx.MainCamera.transform.position, ctx.MainCamera.transform.TransformDirection(Vector3.forward), out hit, 2.0f, layerMask))
        {
            //enable the ability to pick up item, notify the user they can pick up this item.
            Debug.DrawRay(ctx.MainCamera.transform.position, ctx.MainCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            IInteractable interactable = hit.collider.gameObject.GetComponent<IInteractable>();

            if (interactable == null)
            {
                return;
            }

            ctx.InfoText.text = interactable.Message();
            currentSelection = interactable;

            if (ctx.IsInteractInput)
            {
                ctx.IsInteractInput = false;
                interactable.Interact();
                ctx.InfoText.text = "";
            }
        }
        else
        {
            //disable the ability to pick up item. Turn off the notice to pick up the item.
            Debug.DrawRay(ctx.MainCamera.transform.position, ctx.MainCamera.transform.TransformDirection(Vector3.forward) * 2, Color.white);
            ctx.InfoText.text = "";
            if (currentSelection == null) return;

            currentSelection.Deselect();
            currentSelection = null;
        }
    }

    private void Combat()
    {
        if (!isAbsorbing && !isShooting)
        {
            ctx.DreamCatcher.RotateToDefault();
        }

        Absorbing();
        Shooting();
    }

    private void Shooting()
    {
        if (shootingCooldown > 0)
        {
            shootingCooldown -= Time.deltaTime;
            return;
        }

        isShooting = ctx.IsShootInput && !ctx.IsAbsorbInput;

        if (!isShooting) return;

        isShooting = true;
        shootingCooldown = SHOOTINGMAXCOOLDOWN;

        HitTarget(20.0f);
    }

    private void Absorbing()
    {
        isAbsorbing = !isShooting && ctx.IsAbsorbInput;

        if (!isAbsorbing)
        {
            ctx.DreamCatcher.StopAbsorbing();
            return;
        }

        HitTarget(4.0f);
    }

    private void HitTarget(float distance)
    {
        //bit shift layer 7 (Hurtable)
        int layerMask = 1 << 6;

        RaycastHit hit;

        if (Physics.Raycast(ctx.MainCamera.transform.position, ctx.MainCamera.transform.TransformDirection(Vector3.forward), out hit, distance, layerMask))
        {
            Debug.DrawRay(ctx.MainCamera.transform.position, ctx.MainCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            IHitable target = hit.collider.GetComponent<IHitable>();

            if (target == null) return;

            //rotate the dreamcatcher to the target
            ctx.DreamCatcher.RotateToTarget(hit.transform.position);

            if (isAbsorbing)
            {
                ctx.DreamCatcher.StartAbsorbing();

                if (target.Absorb(1.0f))
                {
                    ctx.DreamCatcher.LightUpCrystal();
                }

                return;
            }

            //fire projectile at target
            ctx.DreamCatcher.FireProjectile();

            target.StunDamage(30.0f);
        }
    }

    private void Save()
    {
        if (savingCooldown > 0.0f)
        {
            savingCooldown -= Time.deltaTime;
            return;
        }

        if (ctx.IsQuickSave)
        {
            ctx.IsQuickSave = false;
            savingCooldown = SAVINGMAXCOOLDOWN;
            ctx.GameManager.QuickSave();
        }
    }

    private void Load()
    {
        if (ctx.IsQuickLoad)
        {
            ctx.IsQuickLoad = false;
            ctx.GameManager.QuickLoad();
        }
    }
}
