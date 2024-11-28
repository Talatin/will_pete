
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public static class CheatSystem
    {
        public delegate void NoClipEventHandler(int ID);
        public static event NoClipEventHandler OnNoclipToggled;
        public delegate void InvincibilityEventHandler(int ID);
        public static event InvincibilityEventHandler OnInvincibilityToggled;
        public delegate void ReviveEventHandler(int ID);
        public static event ReviveEventHandler OnReviveTriggered;

        public static void ToggleNoclip(int PlayerID)
        {
            OnNoclipToggled.Invoke(PlayerID);
        }

        public static void ReviveSelf(int PlayerID)
        {
            OnReviveTriggered.Invoke(PlayerID);
        }

        public static void ToggleInvincibility(int PlayerID)
        {
            OnInvincibilityToggled.Invoke(PlayerID);
        }

        public static void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static void LoadMainMenu()
        {
            SceneLoader.LoadScene("MainMenu");
        }
    }
}
