namespace Assets.Scripts.Player
{
    public interface IPlayerMovement
    {
        public void Initialize(PlayerState state, PlayerSettings settings, PlayerInputHandler input, int playerID);
        public void UpdateMovement();
        public bool Jump();
        public void ToggleNoClip(int playerID);
    }
}