using UnityEngine;

public class SpriteAnimationController : MonoBehaviour
{

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }
  
}
