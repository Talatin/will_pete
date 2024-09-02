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
    
    //public bool AimingInput;
    //public bool FireInput;

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

}
