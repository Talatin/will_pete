using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public static class SceneLoader
    {
        private const string LOADING_SCENE_NAME = "LoadingScene";
        private static string nextLevelName;
        public static string NextLevelName { get { return nextLevelName; } }

        public static void LoadTransitionScene()
        {
            if (SceneManager.GetActiveScene().name != LOADING_SCENE_NAME)
            {
                SceneManager.LoadScene(LOADING_SCENE_NAME);
            }
        }

        public static void LoadScene(string sceneName)
        {
            nextLevelName = sceneName;
            LoadTransitionScene();
        }

        public static void ReloadLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
