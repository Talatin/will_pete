using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static string HORIZONTAL_VELOCITY_ID = "xVelocity";
        private static string VERTICAL_VELOCITY_ID = "yVelocity";
        private static string JUMP_ID = "Jump";
        private static string FIRE_ID = "Fire";

        [SerializeField] private Animator weaponAnimator;
        private Animator playerAnimator;

        private SpriteRenderer spRend;
        private Rigidbody2D rb;

        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
            spRend = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        public void UpdateAnimationMoveValues()
        {
            playerAnimator.SetFloat(HORIZONTAL_VELOCITY_ID, Mathf.Abs(rb.velocity.x));
            playerAnimator.SetFloat(VERTICAL_VELOCITY_ID, rb.velocity.y);
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
            if (rb.velocity.x > 0.1f)
            {
                spRend.flipX = false;
            }
            else if (rb.velocity.x < -0.1f)
            {
                spRend.flipX = true;
            }
        }
        void FixedUpdate()
        {
            FlipCharacter();
        }
    }
}