using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerSettings playerSettings;
        private IPlayerMovement playerMovement;
        private IPlayerShooting playerShooting;
        private PlayerInput playerInput;
        private PlayerState playerState;
        private PlayerAnimationController playerAnimationController;
        private PlayerHealth playerHealth;

        private float reviveTimer = 1f;
        private float currentRevTime = 0;

        private void Awake()
        {
            playerState = GetComponent<PlayerState>();
            playerInput = GetComponent<PlayerInput>();
            playerShooting = GetComponent<IPlayerShooting>();
            playerShooting.Initialize(playerState, playerSettings);
            playerMovement = GetComponent<IPlayerMovement>();
            playerMovement.Initialize(playerState, playerSettings, playerInput);
            playerAnimationController = GetComponent<PlayerAnimationController>();
            playerHealth = GetComponent<PlayerHealth>();
            playerState.Init(playerHealth);
        }

        private void Update()
        {

            Vector2 aimDirection = playerState.IsFacingRight ? Vector2.right : Vector2.left;
            aimDirection = playerInput.MovementInput.y > 0.45f ? Vector2.up : aimDirection;
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
                if(playerShooting.Fire(aimDirection))
                {
                    playerAnimationController.PlayFireAnimation();
                }
            }
            playerAnimationController.UpdateAnimationMoveValues();
        }

        private void FixedUpdate()
        {
            playerMovement.UpdateMovement();
            if (playerState.IsGrounded)
            {
                playerHealth.LastStandingPosition = transform.position;
            }
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