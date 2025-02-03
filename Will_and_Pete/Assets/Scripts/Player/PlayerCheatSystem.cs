
namespace Assets.Scripts.Player
{
    internal class PlayerCheatSystem
    {
        int playerID;
        internal PlayerCheatSystem(int playerID)
        {
            this.playerID = playerID;
        }
        internal void Noclip()
        {
            CheatSystem.ToggleNoclip(playerID);
        }

        internal void Reload()
        {
            CheatSystem.ReloadLevel();
        }

        internal void LoadMainMenu()
        {
            CheatSystem.LoadMainMenu();
        }

        internal void Invincibility()
        {
            CheatSystem.ToggleInvincibility(playerID);
        }

    }
}
