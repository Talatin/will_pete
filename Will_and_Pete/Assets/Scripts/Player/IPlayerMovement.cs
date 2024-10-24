namespace Assets.Scripts.Player
{
    public interface IPlayerMovement
    {
        public void Initialize(PlayerState state,PlayerSettings settings, PlayerInput input);
        public void UpdateMovement();
        public bool Jump();
    }
}