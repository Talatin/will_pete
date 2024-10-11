using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    internal class ChaseStrategy : MonoBehaviour, IEnemyMoveStrategy
    {
        public Vector3 LastPlayerPosition;
        [SerializeField] private float distanceToStop;
        [SerializeField] private float jumpForce;
        [SerializeField] float speed;
        [SerializeField] private Transform cliffCheckPos;
        [SerializeField] private float checkRadius;
        [SerializeField] private LayerMask checkLayer;
        private Rigidbody2D rb;
        private bool isGrounded;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        private void FixedUpdate()
        {
            isGrounded = Physics2D.OverlapCircle(new Vector2(transform.position.x, cliffCheckPos.position.y), checkRadius, checkLayer) && rb.velocity.y <= 0;
            GravityManipulation();
        }

        public void Move()
        {
            if (LastPlayerPosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }
            if ((LastPlayerPosition - transform.position).magnitude > distanceToStop)
            {
                rb.velocity = new Vector2(transform.localScale.x * speed * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                rb.velocity = Vector3.zero;
                //InAttackRange
            }
            if (!Physics2D.OverlapCircle(cliffCheckPos.position, checkRadius, checkLayer))
            {
                if (isGrounded)
                {
                    isGrounded = false;
                    Jump(); 
                }
            }
        }

        private void GravityManipulation()
        {
            // Set Charactergravity according to current y velocity and jump input
            if (rb.velocity.y < -0.1f)
            {
                rb.gravityScale = 2;
            }
            else if (rb.velocity.y >= 0)
            {
                rb.gravityScale = 1;
            }
        }

        private void Jump()
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
