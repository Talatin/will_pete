using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        [SerializeField] private MovementSettings settings;
        private Rigidbody2D rb;
        private float defaultGravity;
        private float timeStampJumpBuffer = -1;
        private float timeStampCoyoteBuffer = -1;
        private bool isCoyoteGrounded = false;
        private static float AVATAR_FALL_GRAVITY_MIN_VELOCITY = -0.2f;
        private bool hasJumped;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            defaultGravity = rb.gravityScale;
        }

        public void UpdateMovement(PlayerInput pInput, PlayerState pState)
        {
            Move(pInput, pState);
            JumpAssists(pInput, pState);
            GravityManipulation(pInput);
        }
        public bool Jump(PlayerInput pInput, PlayerState pState)
        {
            if (!isCoyoteGrounded)
            {
                if (timeStampJumpBuffer == 0)
                {
                    timeStampJumpBuffer = Time.time;
                }
                return false;
            }
            timeStampCoyoteBuffer = 0;
            timeStampJumpBuffer = 0;

            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * settings.jumpPower, ForceMode2D.Impulse);
            hasJumped = true;
            return true;
        }

        private void JumpAssists(PlayerInput pInput, PlayerState pState)
        {
            if (Time.time - timeStampJumpBuffer < settings.jumpBufferTime)
            {
                Jump(pInput, pState);
            }

            if (pState.isGrounded && !pState.isStoodOn)
            {
                isCoyoteGrounded = true;
                timeStampCoyoteBuffer = Time.time;
            }
            if (Time.time - timeStampCoyoteBuffer >= settings.coyoteTime)
            {
                isCoyoteGrounded = false;
                timeStampCoyoteBuffer = 0;
            }
        }


        private void Move(PlayerInput pInput, PlayerState pState)
        {
            if (pState.isGrounded) //Ground Movement = Snappy and fast
            {
                rb.velocity = new Vector2(pInput.MovementInput.x * settings.speed * Time.deltaTime, rb.velocity.y);
            }
            else //Air movement = Takes more time to reach same speed or stop | Change airControl to adjust the effect
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(pInput.MovementInput.x * settings.speed * Time.fixedDeltaTime, rb.velocity.y), settings.airControl * Time.deltaTime);
            }
        }

        private void GravityManipulation(PlayerInput pInput)
        {
            // Set Charactergravity according to current y velocity and jump input
            if (rb.velocity.y < AVATAR_FALL_GRAVITY_MIN_VELOCITY)
            {
                hasJumped = false;
                rb.gravityScale = settings.fallMultiplier;
            }
            if (rb.velocity.y > 0 && !pInput.JumpInput && hasJumped)
            {
                rb.gravityScale = settings.lowJumpMultiplier;
            }
            else if (rb.velocity.y >= 0)
            {
                rb.gravityScale = defaultGravity;
            }
        }
    }
}
