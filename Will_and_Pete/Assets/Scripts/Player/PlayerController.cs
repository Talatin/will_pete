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

        private void Awake()
        {
            playerState = GetComponent<PlayerState>();
            playerInput = GetComponent<PlayerInput>();
            playerShooting = GetComponent<IPlayerShooting>();
            playerMovement = GetComponent<IPlayerMovement>();
            playerAnimationController = GetComponent<PlayerAnimationController>();
            PlayerHealth = GetComponent<PlayerHealth>();
        }

        private void Update()
        {
            Vector2 aimDirection = playerState.isFacingRight ? Vector2.right : Vector2.left;
            aimDirection = playerInput.MovementInput.y > 0.45f ? Vector2.up : aimDirection;
            playerShooting.Aim(playerState, aimDirection);

            if (playerInput.JumpInput)
            {
                if (playerMovement.Jump(playerInput, playerState))
                {
                    playerAnimationController.PlayJumpAnimation();
                }
                playerInput.JumpInput = false;
            }
            if (playerInput.FireInput)
            {
                if (playerShooting.Fire(aimDirection))
                {
                    playerAnimationController.PlayFireAnimation();
                }
                playerInput.FireInput = false;
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