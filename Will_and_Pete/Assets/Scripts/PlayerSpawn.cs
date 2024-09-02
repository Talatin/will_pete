using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(PlayerOne, new Vector3(-3, 1, 0), Quaternion.identity);
        Instantiate(PlayerTwo, new Vector3(3, 1, 0), Quaternion.identity);
    }
}
