using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerTwo;
        [SerializeField] private GameObject PlayerOne;
        // Start is called before the first frame update
        void Start()
        {
            Vector3 spawnOffset = new Vector3(3, 0, 0);

            Instantiate(PlayerOne, transform.position - spawnOffset, Quaternion.identity);
            Instantiate(PlayerTwo, transform.position + spawnOffset, Quaternion.identity);
        }
    }
}