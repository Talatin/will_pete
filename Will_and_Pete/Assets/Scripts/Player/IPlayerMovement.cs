namespace Assets.Scripts.Player
{
    public interface IPlayerMovement
    {
        public void UpdateMovement(PlayerInput pInput, PlayerState pState);
        public bool Jump(PlayerInput pInput = null, PlayerState pState = null);
    }
}