using UnityEngine;

namespace World
{
    public interface IKnockable
    {
        void Knockback(Vector2 point, Vector2 origin, float knockbackForce);
    }
}