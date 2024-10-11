using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlantController : MonoBehaviour
{
    [SerializeField] private float jumpPower;
    private Vector3 lineStart;
    private Vector3 lineEnd;
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void CalculateJumpLine()
    {
        float g = 1.5f * Physics2D.gravity.magnitude;
        float v0 = jumpPower / 1; // converts the jumpForce to an initial velocity
        float maxJump_y = transform.position.y + (v0 * v0) / (2 * g);

        // For Debug.DrawLine in FixedUpdate :
        lineStart = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, 0);
        lineEnd = new Vector3(transform.position.x, maxJump_y, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (collision.transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                animator.SetTrigger("Jumped");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        CalculateJumpLine();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(lineStart, lineEnd);
    }

}
