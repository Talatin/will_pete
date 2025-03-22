using UnityEngine;

namespace Assets.Scripts.World
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RockBehaviour : MonoBehaviour, IThrowable
    {
        private Rigidbody2D rb;
        private Collider2D collider2d;

        private void Awake()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            collider2d = gameObject.GetComponent<Collider2D>();
        }

        public void ToggleRigidbody(bool state)
        {
            rb.bodyType = state ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
        }

        public void ToggleCollider(bool state)
        {
            collider2d.enabled = state;
        }

        public void Throw(float throwForce, Vector2 direction)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(throwForce * direction, ForceMode2D.Impulse);
        }

        public Rigidbody2D GetRigidbody()
        {
            return rb;
        }

        public void SetParent(Transform parent)
        {
            //transform.SetParent(parent);
        }
    }
}