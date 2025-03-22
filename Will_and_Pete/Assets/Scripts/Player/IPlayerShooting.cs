using Player;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public interface IPlayerShooting
    {
        public void Initialize(PlayerState state, PlayerSettings settings, CameraBehaviour cameraBehaviour);
        public bool Fire(Vector2 direction);
        public void Aim(Vector2 direction);

        public void ThrowWeapon();
        
        public void ToggleActive();
        void KneelDown(bool value);
    }
}