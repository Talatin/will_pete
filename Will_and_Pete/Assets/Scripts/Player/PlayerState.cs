using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerState : MonoBehaviour
    {

        [SerializeField] private Transform groundCheckPos;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform playerOnTopCheckPos;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Vector2 groundCheckSize;
        [SerializeField] private Vector2 stoodOnCheckSize;
        [SerializeField] private bool showGizmos;

        private Rigidbody2D rb;

        public bool IsGrounded { get ; private set; }
        public bool IsStoodOn { get ; private set; }
        public bool IsDowned { get; private set; }
        public bool IsFacingRight { get; private set; }

        public bool GetisFalling()
        {
            return rb.velocity.y < 0;
        }

        public bool GetisMoving()
        {
            return Mathf.Abs(rb.velocity.x) < 0;
        }

        public void Init(PlayerHealth health)
        {
            health.onDownedStateChanged += onHealthStateChanged;
            IsFacingRight = true;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            IsGrounded = GroundCheck();
            IsStoodOn = StoodOnCheck();
            IsFacingRight = PlayerDirectionCheck();
        }

        private bool GroundCheck()
        {
            return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

        }
        private bool StoodOnCheck()
        {
            return Physics2D.OverlapBox(playerOnTopCheckPos.position, stoodOnCheckSize, 0, playerLayer);

        }

        private void onHealthStateChanged(bool value)
        {
            IsDowned = value;
        }

        private bool PlayerDirectionCheck()
        {
            if (rb.velocity.x < -0.1f)
            {
                return false;
            }
            else if (rb.velocity.x > 0.1f)
            {
                return  true;
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
            }
        }
    }
}