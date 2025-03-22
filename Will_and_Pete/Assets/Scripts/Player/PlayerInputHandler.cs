using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private const string MOUSE_INPUT_NAME = "Mouse";
        public Vector2 MovementInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputHeld { get; private set; }

        public bool InteractInput { get; private set; }

        public bool AbilityOneInput { get; private set; }
        public bool AbilityTwoInput { get; private set; }

        public bool BackPackInput { get; private set; }
        public bool BackPackHeld { get; private set; }

        public bool CrouchInput { get; private set; }
        public Vector2 AimingInput { get; private set; }
        public bool FireInput { get; private set; }
        public bool KneelInput { get; private set; }
        
        public bool Cheat_Toggle { get; private set; }
        public bool Cheat_NoClip { get; private set; }
        public bool Cheat_ReloadLevel { get; private set; }
        public bool Cheat_LoadMainMenu { get; private set; }
        public bool Cheat_Invincibility { get; private set; }

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            
        }

        public void ResetFrameValues()
        {
            AbilityOneInput = false;
            JumpInput = false;
            Cheat_NoClip = false;
            Cheat_LoadMainMenu = false;
            Cheat_ReloadLevel = false;
            Cheat_Invincibility = false;
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
                JumpInputHeld = true;
            }

            if (context.canceled)
            {
                JumpInput = false;
                JumpInputHeld = false;
            }
        }

        public void OnAbilityOne(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AbilityOneInput = true;
            }
            if (context.canceled)
            {
                AbilityOneInput = false;
            }
        }

        public void OnAbilityTwo(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                KneelInput = true;
            }

            if (context.canceled)
            {
                KneelInput = false;
            }
        }
        #region unused
        public void OnBackPack(InputAction.CallbackContext context)
        {
            BackPackInput = context.action.triggered;
        }

        public void OnBackPackHold(InputAction.CallbackContext context)
        {
            BackPackHeld = context.action.triggered;
        }

        #endregion

        public void OnAiming(InputAction.CallbackContext context)
        {
            if (!cam || context.canceled)
            {
                return;
            }
            AimingInput = context.ReadValue<Vector2>();
            if (context.control.device.displayName == MOUSE_INPUT_NAME)
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

        public void OnCheatToggle(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Cheat_Toggle = true;
            }

            if (context.canceled)
            {
                Cheat_Toggle = false;
            }
        }

        public void OnCheatNoClip(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Cheat_NoClip = true;
            }

            if (context.canceled)
            {
                Cheat_NoClip = false;
            }
        }

        public void OnCheatLoadMainMenu(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Cheat_LoadMainMenu = true;
            }

            if (context.canceled)
            {
                Cheat_LoadMainMenu = false;
            }
        }

        public void OnCheatReload(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Cheat_ReloadLevel = true;
            }

            if (context.canceled)
            {
                Cheat_ReloadLevel = false;
            }
        }

        public void OnCheatInvincibility(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Cheat_Invincibility = true;
            }

            if (context.canceled)
            {
                Cheat_Invincibility = false;
            }
        }
    }
}