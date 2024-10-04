using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class GoblinHealth : MonoBehaviour, IDamageable
    {
        Rigidbody2D rb;
        public delegate void Died();
        public event Died died;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage()
        {
            rb.velocity = Vector3.zero;
            rb.drag = 1.25f;
            rb.AddForce(transform.localScale * 4, ForceMode2D.Impulse);
            died?.Invoke();
        }
    }
}