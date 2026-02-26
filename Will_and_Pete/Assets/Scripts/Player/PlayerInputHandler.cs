using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;


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
            jumpAction.performed += _ => JumpInput = true;
            interactAction.performed += OnInteract;
            aimAction.performed += OnAiming;
            fireAction.performed += OnFire;

            abilityOneAction.performed += OnAbilityOne;
            abilityTwoAction.performed += OnAbilityTwo;
            
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
            // MovementInput = movementAction.ReadValue<Vector2>();
            // if (MovementInput.magnitude > 0 && MovementInput.magnitude < 0.2f)
            // {
            //     MovementInput = MovementInput.normalized * 0.2f;
            // }

            JumpInputHeld = jumpAction.ReadValue<float>() > 0.5f;
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

        public void OnMovement(InputAction.CallbackContext context)
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
                AbilityTwoInput = true;
            }

            if (context.canceled)
            {
                AbilityTwoInput = false;
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