using Assets.Scripts.Player;

namespace Player
{
    public interface IPlayerMovement
    {
        public void Initialize(PlayerController player);
        public void UpdateMovement();
        public bool Jump();
        public void ToggleNoClip(int playerID);
    }
}