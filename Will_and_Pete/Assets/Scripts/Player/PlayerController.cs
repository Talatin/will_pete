using Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;
        private IPlayerMovement playerMovement;
        private IPlayerShooting playerShooting;
        private PlayerInputHandler playerInput;
        private PlayerState playerState;
        private PlayerAnimationController playerAnimationController;
        private PlayerHealth playerHealth;
        private PlayerCheatSystem playerCheatSystem;
        private CameraBehaviour cameraBehaviour;
        private PlayerThrowing playerThrowing;
        private int playerID;

        private GameObject cheatUIObject;

        public GameObject CheatUiObject
        {
            set => cheatUIObject = value;
        }

        public CameraBehaviour CameraBehaviour
        {
            set => cameraBehaviour = value;
        }

        private void Awake()
        {
            playerID = Random.Range(1, int.MaxValue);
            playerState = GetComponent<PlayerState>();
            playerInput = GetComponent<PlayerInputHandler>();
            
            playerHealth = GetComponent<PlayerHealth>();
            playerHealth.Initialize(playerSettings);
            
            playerState.Initialize(playerHealth, playerInput);
            
            playerShooting = GetComponent<IPlayerShooting>();
            playerShooting.Initialize(playerState, playerSettings,cameraBehaviour);
            
            playerMovement = GetComponent<IPlayerMovement>();
            playerMovement.Initialize(playerState, playerSettings, playerInput, playerID);
            
            playerAnimationController = GetComponent<PlayerAnimationController>();
            playerAnimationController.Initialize(playerInput);
            
            playerThrowing = GetComponent<PlayerThrowing>();
            playerThrowing.Initialize(playerState, playerSettings);
            
            playerCheatSystem = new PlayerCheatSystem(playerID);
        }

        private void Update()
        {
            // Vector2 aimDirection = playerState.IsFacingRight ? Vector2.right : Vector2.left;
            // aimDirection = playerInput.MovementInput.y > 0.45f ? Vector2.up : aimDirection;
            // aimDirection = playerInput.MovementInput.y < -0.45f ? Vector2.down : aimDirection;
            Vector2 aimDirection = playerInput.AimingInput;
            playerShooting.Aim(aimDirection);
            playerState.SetIsKneeling(playerInput.KneelInput);

            if (playerInput.AbilityOneInput)
            {
                // playerThrowing.Throw();
                playerShooting.ThrowWeapon();
            }
            
            if (playerInput.InteractInput)
            {
                playerHealth.HelpUpPlayer();
            }

            if (playerInput.JumpInput)
            {
                if (playerMovement.Jump())
                {
                    playerAnimationController.PlayJumpAnimation();
                }
            }
            if (playerInput.FireInput)
            {
                if (playerShooting.Fire(aimDirection))
                {
                    playerAnimationController.PlayFireAnimation();
                }
            }
            playerAnimationController.UpdateAnimationMoveValues();

            #region Cheating
#if ENABLE_CHEATS
            if (playerInput.Cheat_Toggle)
            {
                if (!cheatUIObject.activeSelf)
                {
                    cheatUIObject.SetActive(true);
                }
                if (playerInput.Cheat_NoClip)
                {
                    playerCheatSystem.Noclip();
                }
                if (playerInput.Cheat_ReloadLevel)
                {
                    playerCheatSystem.Reload();
                }
                if (playerInput.Cheat_LoadMainMenu)
                {
                    playerCheatSystem.LoadMainMenu();
                }
            }
            else
            {
                if (cheatUIObject.activeSelf)
                {
                    cheatUIObject.SetActive(false);
                }
            }
#endif
            #endregion
            playerInput.ResetFrameValues();
        }

        private void FixedUpdate()
        {
            playerMovement.UpdateMovement();
        }

    }
}