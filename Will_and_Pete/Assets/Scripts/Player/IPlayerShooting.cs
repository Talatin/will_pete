using UnityEngine;

namespace Assets.Scripts.Player
{
    public interface IPlayerShooting
    {
        public bool Fire(Vector2 direction, PlayerState pState);
        public void Aim(PlayerState pState, Vector2 direction);
    }
}