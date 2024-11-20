using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimationBehaviour : MonoBehaviour
{
    public float speed;
    private Camera cam;
    private Bounds bounds;

    private void Awake()
    {
        cam = Camera.main;
        bounds = OrthographicBounds(cam);

    }


    public void UpdateLoadingVisuals(float loadingProgress)
    {
        Vector3 startPos = new Vector3(bounds.min.x, bounds.center.y);
        Vector3 endPos = new Vector3(bounds.max.x, bounds.center.y);
        transform.position = Vector3.Lerp(startPos, endPos, loadingProgress + 0.1f);
        //RandomHeightRunning();
    }

    private void RandomHeightRunning()
    {
        if (transform.position.x > bounds.max.x + 1)
        {
            transform.position = new Vector3(bounds.min.x - 2, Random.Range(bounds.min.y, bounds.max.y));
        }
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    public static Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }

}
