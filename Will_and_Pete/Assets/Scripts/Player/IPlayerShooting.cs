using UnityEngine;

namespace Assets.Scripts.Player
{
    public interface IPlayerShooting
    {
        public bool Fire(Vector2 direction);
    }
}