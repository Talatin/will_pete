using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MovementInput { get; private set; }
        public bool JumpInput { get; private set; }

        public bool InteractInput { get; private set; }

        public bool AbilityOneInput { get; private set; }
        public bool AbilityTwoInput { get; private set; }

        public bool BackPackInput { get; private set; }
        public bool BackPackHeld { get; private set; }

        public bool CrouchInput { get; private set; }
        public Vector2 AimingInput { get; private set; }
        public bool FireInput { get; private set; }

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
            if (context.started)
            {
                JumpInput = true;
            }
            if (context.canceled)
            {
                JumpInput = false;
            }
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
            if (context.started)
            {
                FireInput = true;
            }
            if (context.canceled)
            {
                FireInput = false;
            }
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

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                InteractInput = true;
            }
            if (context.canceled)
            {
                InteractInput = false;
            }
        }

    }
}