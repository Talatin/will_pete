using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene_Management
{
    public class LoadingSceneBehaviour : MonoBehaviour
    {
        private float additionalLoadingTime;
        private LoadingAnimationBehaviour loadingAnimationBehaviour;
        private AsyncOperation operation;
        private float currentLoadingTime;
        private float loadingPerc;

        void Awake()
        {
            loadingAnimationBehaviour = GetComponent<LoadingAnimationBehaviour>();
            additionalLoadingTime = SceneLoader.FakeLoadingTime;
            StartCoroutine(LoadScene());
        }

        private IEnumerator LoadScene()
        {
            yield return null;
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneLoader.NextLevelName);
            asyncOperation.allowSceneActivation = false;
            while (!asyncOperation.isDone)
            {
                currentLoadingTime += Time.deltaTime;
                loadingPerc = currentLoadingTime / additionalLoadingTime;
                loadingPerc *= asyncOperation.progress;
                loadingAnimationBehaviour.UpdateLoadingVisuals(loadingPerc);

                if (loadingPerc >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}
