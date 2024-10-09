using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public enum DamageType { Health, Knockback, InstaKill, reset }
    private Vector3 lastStandingPosition;
    public Vector3 LastStandingPosition { set { lastStandingPosition = value; } }
    private Rigidbody2D rb;
    [SerializeField] private float knockbackPower;
    [SerializeField] private Vector2 knockbackOffset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(DamageType type, Transform source)
    {
        switch (type)
        {
            case DamageType.Health:
                break;
            case DamageType.Knockback:
                KnockbackDamage(source);
                break;
            case DamageType.InstaKill:
                InstaKill();
                break;
            case DamageType.reset:
                ResetPlayer();
                break;
            default:
                break;
        }
    }

    private void HealthDamage()
    {

    }

    private void KnockbackDamage(Transform source)
    {
        Debug.Log($"Knockbacked by {source.name}");
        Vector3 dir = (source.transform.position - transform.position).normalized;
        float xKnockback = Vector2.Dot(dir, transform.right) < 0 ? 1 : -1;
        Vector2 knockDir = new Vector2 (xKnockback, knockbackOffset.y).normalized;

        rb.AddForce(knockDir * knockbackPower, ForceMode2D.Impulse);
    }
    private void InstaKill()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void ResetPlayer()
    {
        rb.velocity = Vector2.zero;
        transform.position = lastStandingPosition;
    }

    private void Update()
    {

    }

}
