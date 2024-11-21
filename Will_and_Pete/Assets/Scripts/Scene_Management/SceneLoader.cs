using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public static class SceneLoader
    {
        private const string LOADING_SCENE_NAME = "LoadingScene";
        private static string nextLevelName;
        private static float fakeLoadingTime;
        public static string NextLevelName { get { return nextLevelName; } }
        public static float FakeLoadingTime { get { return fakeLoadingTime; } }

        public static void LoadTransitionScene()
        {
            if (SceneManager.GetActiveScene().name != LOADING_SCENE_NAME)
            {
                SceneManager.LoadScene(LOADING_SCENE_NAME);
            }
        }

        public static void LoadScene(string sceneName, float additionalLoadTime = 1.5f)
        {
            fakeLoadingTime = Mathf.Max(0.5f, additionalLoadTime);
            nextLevelName = sceneName;
            LoadTransitionScene();
        }

        public static void ReloadLevel()
        {
            LoadScene(SceneManager.GetActiveScene().name,0);
        }
    }
}
