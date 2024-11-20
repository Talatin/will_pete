using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneBehaviour : MonoBehaviour
{
    LoadingAnimationBehaviour loadingAnimationBehaviour;
    AsyncOperation operation;
    private float currentLoadingTime;
    private float loadingPerc;

    public float AdditionalLoadingTime;
    // Start is called before the first frame update
    void Awake()
    {
        loadingAnimationBehaviour = GetComponent<LoadingAnimationBehaviour>();
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
        currentLoadingTime += Time.deltaTime;
        loadingPerc = currentLoadingTime / AdditionalLoadingTime;
    }


    IEnumerator LoadScene()
    {
        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneLoader.NextLevelName);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            loadingPerc *= asyncOperation.progress;
            loadingAnimationBehaviour.UpdateLoadingVisuals(loadingPerc);

            // Check if the load has finished
            if (loadingPerc >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

}
