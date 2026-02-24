using Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static string HORIZONTAL_VELOCITY_ID = "xVelocity";
        private static string VERTICAL_VELOCITY_ID = "yVelocity";
        private static string WALL_SLIDING = "WallSliding";
        private static string JUMP_ID = "Jump";
        private static string FIRE_ID = "Fire";
        private static string KNEEL_ID = "Kneel";

        [SerializeField] private Animator weaponAnimator;
        private Animator playerAnimator;

        private SpriteRenderer spRend;
        private Rigidbody2D rb;
        private PlayerInputHandler playerInput;
        private PlayerState playerState;
        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
            spRend = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(PlayerInputHandler playerInputHandler, PlayerState playerState)
        {
            playerInput = playerInputHandler;
            this.playerState = playerState;
        } 
        

        public void UpdateAnimationMoveValues()
        {
            playerAnimator.SetFloat(HORIZONTAL_VELOCITY_ID, Mathf.Abs(playerInput.MovementInput.x));
            playerAnimator.SetFloat(VERTICAL_VELOCITY_ID, rb.linearVelocity.y);
            playerAnimator.SetBool(WALL_SLIDING,playerState.IsWalledLeft || playerState.IsWalledRight);
        }

        public void PlayFireAnimation()
        {
            weaponAnimator.SetTrigger(FIRE_ID);
        }

        public void PlayJumpAnimation()
        {
            playerAnimator.SetTrigger(JUMP_ID);
        }

        private void FlipCharacter()
        {
            if (playerInput.MovementInput.x > 0.1f)
            {
                spRend.flipX = false;
            }
            else if (playerInput.MovementInput.x < -0.1f)
            {
                spRend.flipX = true;
            }
        }

        private void FixedUpdate()
        {
            FlipCharacter();
        }
    }
}