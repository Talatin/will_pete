using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerState : MonoBehaviour
    {

        [SerializeField] private Transform groundCheckPos;
        [SerializeField] private Transform wallCheckPosRight;
        [SerializeField] private Transform wallCheckPosLeft;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform playerOnTopCheckPos;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Vector2 groundCheckSize;
        [SerializeField] private float wallCheckSize;
        [SerializeField] private Vector2 stoodOnCheckSize;
        [SerializeField] private bool showGizmos;

        private Rigidbody2D rb;
        private PlayerInputHandler playerInput;
        public bool IsGrounded { get; private set; }
        public bool IsStoodOn { get; private set; }
        public bool IsDowned { get; private set; }
        public bool IsFacingRight { get; private set; }
        public bool IsWalledLeft { get; private set; }
        public bool IsWalledRight { get; private set; }
        public bool IsKneeling { get; private set; }
        
        public bool GetIsFalling()
        {
            return rb.velocity.y < 0;
        }

        public bool GetIsMoving()
        {
            return Mathf.Abs(rb.velocity.x) < 0;
        }

        public void Initialize(PlayerHealth health,PlayerInputHandler playerInputHandler)
        {
            health.onDownedStateChanged += onHealthStateChanged;
            playerInput = playerInputHandler;
            IsFacingRight = true;
        }

        public void SetIsKneeling(bool isKneeling)
        {
            IsKneeling = isKneeling;
        }
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            IsStoodOn = StoodOnCheck();
            IsGrounded = GroundCheck();
            IsWalledLeft = WallCheckLeft();
            IsWalledRight = WallCheckright();
            IsFacingRight = PlayerDirectionCheck();
        }

        private bool GroundCheck()
        {
            var result = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
            if (!result)
            {
                return false;
            }

            float closestY = result.ClosestPoint(transform.position).y;
            
            if(closestY < transform.position.y)
            {
                return true;
            }
            
            return false;
        }

        private bool WallCheckLeft()
        {
            return Physics2D.OverlapCircle(wallCheckPosLeft.position, wallCheckSize, groundLayer);
        }

        private bool WallCheckright()
        {
            return Physics2D.OverlapCircle(wallCheckPosRight.position, wallCheckSize, groundLayer);
        }

        private bool StoodOnCheck()
        {
            var result = Physics2D.OverlapBox(playerOnTopCheckPos.position, stoodOnCheckSize, 0, playerLayer);
            try
            {
                return result;
            }
            catch (System.Exception)
            {

                return result;
            }
        }

        private void onHealthStateChanged(bool value)
        {
            IsDowned = value;
        }

        private bool PlayerDirectionCheck()
        {
            if (playerInput.MovementInput.x < -0.001f)
            {
                return false;
            }
            if (playerInput.MovementInput.x > 0.001f)
            {
                return true;
            }
            return IsFacingRight;
        }

        private void OnDrawGizmos()
        {
            if (showGizmos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(playerOnTopCheckPos.position, stoodOnCheckSize);
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(wallCheckPosRight.position, wallCheckSize);
                Gizmos.DrawWireSphere(wallCheckPosLeft.position, wallCheckSize);
            }
        }
    }
}