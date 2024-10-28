using UnityEngine;

public class JumpPlantController : MonoBehaviour
{
    [SerializeField] private float launchPower;
    private Vector3 lineStart;
    private Vector3 lineEnd;
    private const float PLAYER_MASS = 1;
    private Rigidbody2D launchedRB;

    private void CalculateJumpLine()
    {
        float g = PLAYER_MASS * Physics2D.gravity.magnitude;
        float v0 = launchPower / 1; // converts the jumpForce to an initial velocity
        float maxJump_y = transform.position.y + (v0 * v0) / (2 * g);

        // For Debug.DrawLine in FixedUpdate :
        lineStart = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, 0);
        lineEnd = new Vector3(transform.position.x, maxJump_y, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (!(collision.contacts[0].point.y > transform.position.y + transform.localScale.y / 2))
            {
                return;
            }
            if (collision.transform.TryGetComponent<Rigidbody2D>(out launchedRB))
            {
                launchedRB.AddForce(Vector2.up * launchPower, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        CalculateJumpLine();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(lineStart, lineEnd);
        Gizmos.DrawLine(new Vector3(lineEnd.x - 1, lineEnd.y), new Vector3(lineEnd.x + 1, lineEnd.y));
    }

}
