using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        private IPlayerMovement playerMovement;
        private IPlayerShooting playerShooting;
        private PlayerInput playerInput;
        private PlayerState playerState;
        private PlayerAnimationController playerAnimationController;
        private PlayerHealth PlayerHealth;
        [SerializeField] private LayerMask playerLayer;

        private float reviveTimer = 1f;
        private float currentRevTime = 0;
        private void Awake()
        {
            playerState = GetComponent<PlayerState>();
            playerInput = GetComponent<PlayerInput>();
            playerShooting = GetComponent<IPlayerShooting>();
            playerMovement = GetComponent<IPlayerMovement>();
            playerAnimationController = GetComponent<PlayerAnimationController>();
            PlayerHealth = GetComponent<PlayerHealth>();
            playerState.Init(PlayerHealth);
        }

        private void Update()
        {

            Vector2 aimDirection = playerState.isFacingRight ? Vector2.right : Vector2.left;
            aimDirection = playerInput.MovementInput.y > 0.45f ? Vector2.up : aimDirection;
            playerShooting.Aim(playerState, aimDirection);

            if (playerInput.InteractInput)
            {
                var temp = Physics2D.OverlapCircleAll(transform.position, 2, playerLayer);

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

            if (playerInput.JumpInput)
            {
                if (playerMovement.Jump(playerInput, playerState))
                {
                    playerAnimationController.PlayJumpAnimation();
                }
            }
            if (playerInput.FireInput)
            {
                playerShooting.Fire(aimDirection, playerState);
            }
            playerAnimationController.UpdateAnimations(playerState, playerInput);
        }

        private void FixedUpdate()
        {
            playerMovement.UpdateMovement(playerInput, playerState);
            if (playerState.isGrounded)
            {
                PlayerHealth.LastStandingPosition = transform.position;
            }
        }
    }
}