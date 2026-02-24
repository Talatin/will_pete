using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        private static float AVATAR_FALL_GRAVITY_MIN_VELOCITY = -0.2f;

        private PlayerSettings pSettings;
        private PlayerState pState;
        private PlayerInputHandler pInput;
        private Rigidbody2D rb;
        private BoxCollider2D boxCollider;

        private float defaultGravity;
        private float timeStampJumpBuffer = -1;
        private float timeStampCoyoteBuffer = -1;
        private bool isCoyoteGrounded = false;
        private bool hasJumped;
        private int doubleJumpsAvailable;

        private bool isNoClipping = false;
        private int myPlayerID;
        private bool useGravity = true;

        private float wallJumpRecoveryCurrentTime;
        private float airControlFactor = 1;

        private float fallTime;


        public void Initialize(PlayerState state, PlayerSettings settings, PlayerInputHandler input, int playerID)
        {
            pSettings = settings;
            pState = state;
            pInput = input;
            myPlayerID = playerID;
            CheatSystem.OnNoclipToggled += ToggleNoClip;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            boxCollider = GetComponent<BoxCollider2D>();
            defaultGravity = rb.gravityScale;
        }


        public void UpdateMovement()
        {
            if (isNoClipping)
            {
                MoveNoClip();
                return;
            }

            if (pState.IsDowned)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
                return;
            }

            Move();
            JumpAssists();
            WallSlide();
            WallJumpRecovery();
            GravityManipulation();
        }

        public void ToggleNoClip(int playerID)
        {
            if (myPlayerID != playerID)
            {
                return;
            }

            isNoClipping = !isNoClipping;
            rb.bodyType = isNoClipping ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
            rb.gravityScale = isNoClipping ? 0 : defaultGravity;
            boxCollider.enabled = !isNoClipping;
        }

        

        private void Move()
        {
            if (pState.IsGrounded) //Ground Movement = Snappy and fast
            {
                rb.linearVelocity = new Vector2(pInput.MovementInput.x * pSettings.Speed * Time.deltaTime, rb.linearVelocity.y);
            }
            else //Air movement = Takes more time to reach same speed or stop | Change airControl to adjust the effect
            {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity,
                    new Vector2(pInput.MovementInput.x * pSettings.Speed * Time.fixedDeltaTime, rb.linearVelocity.y),
                    (pSettings.AirControl * airControlFactor) * Time.deltaTime);
            }

            if (rb.linearVelocity.y < -pSettings.FallingSpeedCap)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -pSettings.FallingSpeedCap);
            }
        }

        private void MoveNoClip()
        {
            rb.linearVelocity = pInput.MovementInput * (pSettings.Speed * 2 * Time.deltaTime);
        }

        public bool Jump()
        {
            if (isNoClipping)
            {
                return false;
            }

            if (pState.IsDowned)
            {
                return false;
            }


            if (isCoyoteGrounded)
            {
                //Setting velocity.y to 0 so the character doesn't struggle against gravity.
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
                rb.AddForce(Vector2.up * pSettings.JumpPower, ForceMode2D.Impulse);
                timeStampJumpBuffer = 0;
            }
            // else if (pState.IsWalledLeft)
            // {
            //     Vector2 calculatedJumpDir = pSettings.WallJumpDirection;
            //     rb.linearVelocity = Vector2.zero;
            //     rb.AddForce(calculatedJumpDir * pSettings.WallJumpPower, ForceMode2D.Impulse);
            //     wallJumpRecoveryCurrentTime = 0;
            //     timeStampJumpBuffer = 0;
            // }
            // else if (pState.IsWalledRight)
            // {
            //     Vector2 calculatedJumpDir =
            //         new Vector2(pSettings.WallJumpDirection.x * -1, pSettings.WallJumpDirection.y);
            //     rb.linearVelocity = Vector2.zero;
            //     rb.AddForce(calculatedJumpDir * pSettings.WallJumpPower, ForceMode2D.Impulse);
            //     wallJumpRecoveryCurrentTime = 0;
            //     timeStampJumpBuffer = 0;
            // }
            // else if (doubleJumpsAvailable > 0 && (!pState.IsWalledLeft && !pState.IsWalledRight))
            // {
            //     //Setting velocity.y to 0 so the character doesnt struggle against gravity.
            //     if (rb.linearVelocity.y < 0)
            //     {
            //         rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            //     }
            //     else
            //     {
            //         rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y / 5);
            //     }
            //
            //     rb.gravityScale = defaultGravity;
            //     rb.AddForce(Vector2.up * pSettings.JumpPower, ForceMode2D.Impulse);
            //     doubleJumpsAvailable -= 1;
            //     timeStampJumpBuffer = 0;
            // }
            else if (timeStampJumpBuffer == 0)
            {
                timeStampJumpBuffer = Time.time;
                return false;
            }

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

            if ((pState.IsGrounded) && !pState.IsStoodOn)
            {
                isCoyoteGrounded = true;
                timeStampCoyoteBuffer = Time.time;
                doubleJumpsAvailable = pSettings.DoubleJumps;
            }

            if (pState.IsWalledLeft || pState.IsWalledRight)
            {
                if (pSettings.ResetDoubleJumpsOnWall)
                {
                    doubleJumpsAvailable = pSettings.DoubleJumps;
                }
            }

            if (Time.time - timeStampCoyoteBuffer >= pSettings.CoyoteTime)
            {
                isCoyoteGrounded = false;
                timeStampCoyoteBuffer = 0;
            }
        }

        private void WallSlide()
        {
            if ((pState.IsWalledRight && pInput.MovementInput.x >= 0.5f ||
                 pState.IsWalledLeft && pInput.MovementInput.x <= -0.5f) && rb.linearVelocity.y < -0.1)
            {
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, new Vector2(rb.linearVelocity.x, pSettings.WallSlideSpeed),
                    Time.fixedDeltaTime * pSettings.WallSlideForce);
                rb.gravityScale = 0;
                useGravity = false;
            }
            else
            {
                useGravity = true;
            }
        }

        private void GravityManipulation()
        {
            if (!useGravity)
            {
                return;
            }

            // Set Charactergravity according to current y velocity and jump input
            if (rb.linearVelocity.y < AVATAR_FALL_GRAVITY_MIN_VELOCITY)
            {
                hasJumped = false;
                rb.gravityScale = pSettings.FallMultiplier;
            }

            if (rb.linearVelocity.y > 0 && !pInput.JumpInputHeld && hasJumped)
            {
                rb.gravityScale = pSettings.LowJumpMultiplier;
            }
            else if (rb.linearVelocity.y >= 0)
            {
                rb.gravityScale = defaultGravity;
            }
        }

        private void WallJumpRecovery()
        {
            wallJumpRecoveryCurrentTime += Time.deltaTime;
            if (wallJumpRecoveryCurrentTime > pSettings.WallJumpStunTime)
            {
                wallJumpRecoveryCurrentTime = pSettings.WallJumpStunTime;
            }

            airControlFactor = pSettings.WallJumpStunRecoveryCurve.Evaluate(wallJumpRecoveryCurrentTime / pSettings.WallJumpStunTime);
        }
    }
}