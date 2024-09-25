using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MovementInput;
    public bool JumpInput;
    public bool JumpHeld;

    public bool AbilityOneInput;
    public bool AbilityTwoInput;
    
    public bool BackPackInput;
    public bool BackPackHeld;

    public bool CrouchInput;
    public Vector2 AimingInput;
    public bool FireInput;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        MovementInput = context.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        JumpInput = context.action.triggered;
        JumpHeld = context.action.triggered;
    }
    
    public void OnAbilityOne(InputAction.CallbackContext context)
    {
        AbilityOneInput = context.action.triggered;
    }

    public void OnAbilityTwo(InputAction.CallbackContext context)
    {
        AbilityTwoInput = context.action.triggered;
    }

    public void OnBackPack(InputAction.CallbackContext context)
    {
        BackPackInput = context.action.triggered;
    }
    
    public void OnBackPackHold(InputAction.CallbackContext context)
    {
        BackPackHeld = context.action.triggered;
    }

    public void OnAiming(InputAction.CallbackContext context)
    {
        AimingInput = context.ReadValue<Vector2>();
        if (context.control.device.displayName == "Mouse")
        {
            Vector2 aimDirection = cam.ScreenToWorldPoint(Input.mousePosition);
            AimingInput = (Vector3)aimDirection - transform.position;
        }
        if (AimingInput.magnitude > 1)
        {
            AimingInput.Normalize();
        }

    }
    public void OnFire(InputAction.CallbackContext context)
    {
        FireInput = context.action.triggered;
    }
    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CrouchInput = true;
        }
        if (context.canceled)
        {
            CrouchInput = false;
        }
    }

}
