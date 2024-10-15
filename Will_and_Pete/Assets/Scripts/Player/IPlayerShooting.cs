using UnityEngine;

namespace Assets.Scripts.Player
{
    public interface IPlayerShooting
    {
        public bool Fire(Vector2 direction);
        public void Aim(PlayerState pState, Vector2 direction);
    }
}