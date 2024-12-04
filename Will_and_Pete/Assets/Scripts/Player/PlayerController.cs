using UnityEngine;
using UnityEngine.InputSystem;

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

        private int playerID;
        private float reviveTimer = 1f;
        private float currentRevTime = 0;
        private GameObject cheatUIObject;

        public GameObject CheatUiObject
        {
            set { cheatUIObject = value; }
        }

        private void Awake()
        {
            playerID = Random.Range(1, int.MaxValue);
            playerState = GetComponent<PlayerState>();
            playerInput = GetComponent<PlayerInputHandler>();
            playerShooting = GetComponent<IPlayerShooting>();
            playerShooting.Initialize(playerState, playerSettings);
            playerMovement = GetComponent<IPlayerMovement>();
            playerMovement.Initialize(playerState, playerSettings, playerInput, playerID);
            playerAnimationController = GetComponent<PlayerAnimationController>();
            playerHealth = GetComponent<PlayerHealth>();
            playerState.Init(playerHealth);
            playerCheatSystem = new PlayerCheatSystem(playerID);
            
        }



        private void Update()
        {

            Vector2 aimDirection = playerState.IsFacingRight ? Vector2.right : Vector2.left;
            aimDirection = playerInput.MovementInput.y > 0.45f ? Vector2.up : aimDirection;
            aimDirection = playerInput.MovementInput.y < -0.45f ? Vector2.down : aimDirection;
            playerShooting.Aim(aimDirection);

            if (playerInput.InteractInput)
            {
                HelpUpPlayer();
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
        }

        private void FixedUpdate()
        {
            playerMovement.UpdateMovement();
        }



        private void HelpUpPlayer()
        {
            var temp = Physics2D.OverlapCircleAll(transform.position, 2, playerSettings.PlayerLayer);
            if (temp.Length >= 2)
            {
                currentRevTime += Time.deltaTime;
                if (currentRevTime > reviveTimer)
                {
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (temp[i].gameObject != this.gameObject)
                        {
                            temp[i].GetComponent<PlayerHealth>().HelpBackUp();
                        }
                    }
                }
            }
        }
    }
}