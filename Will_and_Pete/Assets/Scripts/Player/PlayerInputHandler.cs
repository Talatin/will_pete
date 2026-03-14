using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;

        private const string MOUSE_INPUT_NAME = "Mouse";

        public Vector2 MovementInput { get; private set; }
        public bool JumpInputHeld { get; private set; }

        public event Action InteractEvent;
        public event Action AbilityOneEvent;
        public event Action AbilityTwoEvent;
        public event Action FireEvent;
        public event Action JumpEvent;

        public Vector2 AimingInput { get; private set; }
        public bool FireInput { get; private set; }

        public bool Cheat_Toggle { get; private set; }
        public bool Cheat_NoClip { get; private set; }
        public bool Cheat_ReloadLevel { get; private set; }
        public bool Cheat_LoadMainMenu { get; private set; }
        public bool Cheat_Invincibility { get; private set; }

        private Camera cam;

        private InputActionMap playerActionMap;
        private InputAction movementAction;
        private InputAction jumpAction;
        private InputAction interactAction;
        private InputAction aimAction;
        private InputAction fireAction;
        private InputAction abilityOneAction;
        private InputAction abilityTwoAction;

#if ENABLE_CHEATS
        private InputActionMap cheatingActionMap;
        private InputAction enableCheatsAction;
        private InputAction toggleNoClipAction;
        private InputAction reloadLevelAction;
        private InputAction loadMainMenuAction;
#endif

        private void SetUpInputs()
        {
            playerActionMap = playerInput.actions.FindActionMap("Player");
            playerActionMap.Enable();

            movementAction = playerInput.actions.FindAction("Movement");
            jumpAction = playerInput.actions.FindAction("Jump");
            interactAction = playerInput.actions.FindAction("Interact");
            aimAction = playerInput.actions.FindAction("Aim");
            fireAction = playerInput.actions.FindAction("Fire");
            abilityOneAction = playerInput.actions.FindAction("AbilityOne");
            abilityTwoAction = playerInput.actions.FindAction("AbilityTwo");

            movementAction.performed += OnMovement;
            jumpAction.performed += _ => JumpEvent?.Invoke();
            interactAction.performed += _ => InteractEvent?.Invoke();
            aimAction.performed += OnAiming;
            fireAction.performed += _ => FireEvent?.Invoke();

            abilityOneAction.performed += _ => AbilityOneEvent?.Invoke();
            abilityTwoAction.performed += _ => AbilityTwoEvent?.Invoke();

#if ENABLE_CHEATS

            cheatingActionMap = playerInput.actions.FindActionMap("Cheating");
            cheatingActionMap.Enable();

            enableCheatsAction = playerInput.actions.FindAction("EnableCheats");
            toggleNoClipAction = playerInput.actions.FindAction("ToggleNoClip");
            reloadLevelAction = playerInput.actions.FindAction("ReloadLevel");
            loadMainMenuAction = playerInput.actions.FindAction("LoadMainMenu");

            enableCheatsAction.performed += OnCheatToggle;
            toggleNoClipAction.performed += OnCheatNoClip;
            reloadLevelAction.performed += OnCheatReload;
            loadMainMenuAction.performed += OnCheatLoadMainMenu;
#endif
        }

        private void Awake()
        {
            cam = Camera.main;
            SetUpInputs();
        }

        private void Update()
        {
            JumpInputHeld = jumpAction.ReadValue<float>() > 0.5f;
        }

        private void LateUpdate()
        {
            ResetFrameValues();
        }

        private void OnDestroy()
        {
            InteractEvent = null;
            AbilityOneEvent = null;
            AbilityTwoEvent = null;
            FireEvent = null;
            JumpEvent = null;
        }

        private void ResetFrameValues()
        {
            Cheat_NoClip = false;
            Cheat_LoadMainMenu = false;
            Cheat_ReloadLevel = false;
            Cheat_Invincibility = false;
            FireInput = false;
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MovementInput = context.ReadValue<Vector2>();
            }

            if (context.canceled)
            {
                MovementInput = Vector2.zero;
            }
        }

        private void OnAiming(InputAction.CallbackContext context)
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

        private void OnCheatToggle(InputAction.CallbackContext context)
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

        private void OnCheatNoClip(InputAction.CallbackContext context)
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

        private void OnCheatLoadMainMenu(InputAction.CallbackContext context)
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

        private void OnCheatReload(InputAction.CallbackContext context)
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

        private void OnCheatInvincibility(InputAction.CallbackContext context)
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