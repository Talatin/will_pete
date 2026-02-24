using UnityEngine;

namespace Enemies
{
    public class GoblinHealth : MonoBehaviour, IDamageable
    {
        public delegate void Died();
        public event Died died;
        
        private Rigidbody2D rb;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage()
        {
            rb.linearVelocity = Vector3.zero;
            rb.linearDamping = 4f;
            rb.AddForce(new Vector2(-transform.localScale.x * 3, 5), ForceMode2D.Impulse);
            died?.Invoke();
        }
    }
}