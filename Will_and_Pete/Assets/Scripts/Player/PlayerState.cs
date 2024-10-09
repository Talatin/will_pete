using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform playerOnTopCheckPos;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private Vector2 stoodOnCheckSize;
    [SerializeField] private bool showGizmos;

    public bool isGrounded;
    public bool isStoodOn;
    public bool isFalling => rb.velocity.y < 0;
    public bool isMoving => Mathf.Abs(rb.velocity.x) < 0;
    public bool isFacingRight = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        isGrounded = GroundCheck();
        isStoodOn = StoodOnCheck();
        PlayerDirectionCheck(ref isFacingRight);
    }

    private bool GroundCheck()
    {
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);

    }
    private bool StoodOnCheck()
    {
        return Physics2D.OverlapBox(playerOnTopCheckPos.position, stoodOnCheckSize, 0, playerLayer);

    }

    private void PlayerDirectionCheck(ref bool isFacingRight)
    {
        if (rb.velocity.x < 0)
        {
            isFacingRight = false;
        }
        else if (rb.velocity.x > 0)
        {
            isFacingRight = true;
        }
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
