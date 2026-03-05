using Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;
        private PlayerMovement playerMovement;
        private PlayerInputHandler playerInput;
        private  PlayerState playerState;
        private PlayerAnimationController playerAnimationController;
        private PlayerHealth playerHealth;
        private PlayerCheatSystem playerCheatSystem;
        private CameraBehaviour cameraBehaviour;
         private ParachuteComponent parachute;
        private PlayerItemHandler playerItemHandler;
        public int playerID;
        public Rigidbody2D rb;

        private GameObject cheatUIObject;

        public PlayerInputHandler PlayerInput => playerInput;
        public PlayerState PlayerState => playerState;
        public PlayerSettings PlayerSettings => playerSettings;
        public PlayerAnimationController PlayerAnimationController => playerAnimationController;

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
            rb = GetComponent<Rigidbody2D>();
            playerID = Random.Range(1, int.MaxValue);
           
            playerInput = GetComponent<PlayerInputHandler>();
            
            playerHealth = GetComponent<PlayerHealth>();
            playerHealth.Initialize(playerSettings);
            
            playerState = GetComponent<PlayerState>();
            playerState.Initialize(playerHealth, playerInput);
            
            playerAnimationController = GetComponent<PlayerAnimationController>();
            playerAnimationController.Initialize(playerInput, playerState);

            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.Initialize(this);

            playerItemHandler = GetComponent<PlayerItemHandler>();
            playerItemHandler.Initialize(this);

            parachute = GetComponent<ParachuteComponent>();
            parachute.Initialize(this);
            
            
            playerCheatSystem = new PlayerCheatSystem(playerID);

        }

        private void Update()
        {
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
    }
}