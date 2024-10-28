using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        private static float AVATAR_FALL_GRAVITY_MIN_VELOCITY = -0.2f;

        private PlayerSettings pSettings;
        private PlayerState pState;
        private PlayerInput pInput;
        private Rigidbody2D rb;

        private float defaultGravity;
        private float timeStampJumpBuffer = -1;
        private float timeStampCoyoteBuffer = -1;
        private bool isCoyoteGrounded = false;
        private bool hasJumped;

        public void Initialize(PlayerState state, PlayerSettings settings, PlayerInput input)
        {
            pSettings = settings;
            pState = state;
            pInput = input;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            defaultGravity = rb.gravityScale;
        }


        public void UpdateMovement()
        {
            if (pState.IsDowned)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                return;
            }
            Move();
            JumpAssists();
            GravityManipulation();
        }

        public bool Jump()
        {
            if (pState.IsDowned)
            { return false; }
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
            rb.AddForce(Vector2.up * pSettings.JumpPower, ForceMode2D.Impulse);
            hasJumped = true;
            return true;
        }
        /// <summary>
        /// Coyote time and Jump Input Buffer
        /// </summary>
        private void JumpAssists()
        {
            if (Time.time - timeStampJumpBuffer < pSettings.JumpBufferTime)
            {
                Jump();
            }

            if (pState.IsGrounded && !pState.IsStoodOn)
            {
                isCoyoteGrounded = true;
                timeStampCoyoteBuffer = Time.time;
            }
            if (Time.time - timeStampCoyoteBuffer >= pSettings.CoyoteTime)
            {
                isCoyoteGrounded = false;
                timeStampCoyoteBuffer = 0;
            }
        }

        private void Move()
        {
            if (pState.IsGrounded) //Ground Movement = Snappy and fast
            {
                rb.velocity = new Vector2(pInput.MovementInput.x * pSettings.Speed * Time.deltaTime, rb.velocity.y);
            }
            else //Air movement = Takes more time to reach same speed or stop | Change airControl to adjust the effect
            {
                rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(pInput.MovementInput.x * pSettings.Speed * Time.fixedDeltaTime, rb.velocity.y), pSettings.AirControl * Time.deltaTime);
            }
        }

        private void GravityManipulation()
        {
            // Set Charactergravity according to current y velocity and jump input
            if (rb.velocity.y < AVATAR_FALL_GRAVITY_MIN_VELOCITY)
            {
                hasJumped = false;
                rb.gravityScale = pSettings.FallMultiplier;
            }
            if (rb.velocity.y > 0 && !pInput.JumpInput && hasJumped)
            {
                rb.gravityScale = pSettings.LowJumpMultiplier;
            }
            else if (rb.velocity.y >= 0)
            {
                rb.gravityScale = defaultGravity;
            }
        }
    }
}
