using UnityEngine;

namespace World
{
    public class KnockbackObject : MonoBehaviour, IKnockable
    {
        private Rigidbody2D rb;
        public void Knockback(Vector2 point, Vector2 origin, float knockbackForce)
        {
            if (!rb)
            {
                rb = transform.GetComponent<Rigidbody2D>();
            }

            Vector2 direction = point - origin;
            rb.AddForceAtPosition(direction.normalized * knockbackForce,point, ForceMode2D.Impulse);
        }
    }
}