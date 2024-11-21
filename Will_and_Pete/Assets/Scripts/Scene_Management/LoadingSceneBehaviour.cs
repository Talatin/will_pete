using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Update()
    {
        currentLoadingTime += Time.deltaTime;
        loadingPerc = currentLoadingTime / additionalLoadingTime;
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneLoader.NextLevelName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
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
