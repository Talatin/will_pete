using UnityEngine;

namespace World
{
    public interface IThrowable
    {
        public void ToggleRigidbody(bool state);
        public void ToggleCollider(bool state);
        public void Throw(float throwForce, Vector2 direction);
        public Rigidbody2D GetRigidbody();
        public void SetParent(Transform parent);
    }
}