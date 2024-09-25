using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject PlayerOne;
    public GameObject PlayerTwo;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 spawnOffset = new Vector3(3, 0, 0);

        Instantiate(PlayerOne, transform.position - spawnOffset, Quaternion.identity);
        Instantiate(PlayerTwo, transform.position + spawnOffset, Quaternion.identity);
    }
}
