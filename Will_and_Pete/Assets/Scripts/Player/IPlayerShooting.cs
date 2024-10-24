using UnityEngine;

namespace Assets.Scripts.Player
{
    public interface IPlayerShooting
    {
        public void Initialize(PlayerState state, PlayerSettings settings);
        public bool Fire(Vector2 direction);
        public void Aim(Vector2 direction);
    }
}