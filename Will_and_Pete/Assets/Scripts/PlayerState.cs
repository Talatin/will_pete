using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerOnTopCheckPos;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private bool showGizmos;

    public bool isGrounded;
    public bool isFalling => rb.velocity.y < 0;
    public bool isMoving => Mathf.Abs(rb.velocity.x) < 0;
    public bool isFacingRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        isGrounded = GroundCheck();
        isFacingRight = PlayerDirectionCheck();
    }

    private bool GroundCheck()
    {
        bool groundFound = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize,0, groundLayer);
        bool playerFound = Physics2D.OverlapBox(playerOnTopCheckPos.position, groundCheckSize,0, playerLayer);
        return groundFound && !playerFound;
    }

    private bool PlayerDirectionCheck()
    {
        if (rb.velocity.x < 0)
        {
            return false;
        }
        else if (rb.velocity.x > 0)
        {
            return true;
        }
        return isFacingRight;
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize );
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(playerOnTopCheckPos.position, groundCheckSize);
        }
    }
}
