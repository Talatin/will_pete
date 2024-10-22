using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static string HORIZONTAL_VELOCITY_ID = "xVelocity";
        private static string VERTICAL_VELOCITY_ID = "yVelocity";
        private static string JUMP_ID = "Jump";
        private static string FIRE_ID = "Fire";

        private Animator playAnimator;
        [SerializeField] private Animator weaponAnimator;

        private SpriteRenderer spRend;
        [SerializeField] private Rigidbody2D rb;
        private void Awake()
        {
            playAnimator = GetComponent<Animator>();
            spRend = GetComponent<SpriteRenderer>();
        }



        public void UpdateAnimations(PlayerState pState, PlayerInput pInput)
        {
            playAnimator.SetFloat(HORIZONTAL_VELOCITY_ID, Mathf.Abs(rb.velocity.x));
            playAnimator.SetFloat(VERTICAL_VELOCITY_ID, rb.velocity.y);
        }

        public void PlayFireAnimation()
        {
            weaponAnimator.SetTrigger(FIRE_ID);
        }

        public void PlayJumpAnimation()
        {
            playAnimator.SetTrigger(JUMP_ID);
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