using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        PlayerControls playerControls; 
        
        
        private const string MOUSE_INPUT_NAME = "Mouse";
        public Vector2 MovementInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputHeld { get; private set; }

        public bool InteractInput { get; private set; }

        public bool AbilityOneInput { get; private set; }
        public bool AbilityTwoInput { get; private set; }

        public Vector2 AimingInput { get; private set; }
        public bool FireInput { get; private set; }
        
        public bool Cheat_Toggle { get; private set; }
        public bool Cheat_NoClip { get; private set; }
        public bool Cheat_ReloadLevel { get; private set; }
        public bool Cheat_LoadMainMenu { get; private set; }
        public bool Cheat_Invincibility { get; private set; }

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
            playerControls = new PlayerControls();
            playerControls.Enable();
            playerControls.Player.Aim.performed += OnAiming;
            playerControls.Player.Interact.performed += OnInteract;
            playerControls.Player.Fire.performed += OnFire;
            playerControls.Player.Jump.performed += _ => JumpInput = true;
            
            playerControls.Player.AbilityOne.performed += OnAbilityOne;
            playerControls.Player.AbilityTwo.performed += OnAbilityTwo;
            
#if ENABLE_CHEATS
            playerControls.Cheating.Enable();
            playerControls.Player.EnableCheats.performed += OnCheatToggle;
            playerControls.Player.ToggleNoClip.performed += OnCheatNoClip;
            playerControls.Player.ReloadLevel.performed += OnCheatReload;
            playerControls.Player.LoadMainMenu.performed += OnCheatLoadMainMenu;
#endif

        }

        private void Update()
        {
            MovementInput = playerControls.Player.Movement.ReadValue<Vector2>();
            if (MovementInput.magnitude > 0 && MovementInput.magnitude < 0.2f)
            {
                MovementInput = MovementInput.normalized * 0.2f;
            }
            JumpInputHeld = playerControls.Player.Jump.ReadValue<float>() > 0.5f;
        }

        private void LateUpdate()
        {
            ResetFrameValues();
        }

        private void ResetFrameValues()
        {
            AbilityOneInput = false;
            JumpInput = false;
            Cheat_NoClip = false;
            Cheat_LoadMainMenu = false;
            Cheat_ReloadLevel = false;
            Cheat_Invincibility = false;
            FireInput = false;
        }
        
        public void OnAbilityOne(InputAction.CallbackContext context)
        {
            if (context.performed)
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
            if (context.performed)
            {
            }

            if (context.canceled)
            {
            }
        }
      
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
            if (context.performed)
            {
                FireInput = true;
            }

            if (context.canceled)
            {
                FireInput = false;
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
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
            if (context.performed)
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
            if (context.performed)
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
            if (context.performed)
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
            if (context.performed)
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
            if (context.performed)
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