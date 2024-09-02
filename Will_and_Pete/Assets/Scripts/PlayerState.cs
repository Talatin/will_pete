using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool showGizmos;

    public bool isGrounded => GroundCheck();
    public bool isFalling => rb.velocity.y < 0;
    public bool isMoving => Mathf.Abs(rb.velocity.x) < 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, groundCheckRadius, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPos.position, groundCheckRadius); 
        }
    }
}
