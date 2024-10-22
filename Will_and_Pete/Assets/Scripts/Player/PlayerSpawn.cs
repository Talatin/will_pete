using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerTwo;
        [SerializeField] private GameObject PlayerOne;

        [SerializeField] private Transform P1SpawnPos;
        [SerializeField] private Transform P2SpawnPos;

        public bool isSinglePlayer;
        // Start is called before the first frame update
        void Start()
        {
            Instantiate(PlayerOne, P1SpawnPos.position, Quaternion.identity);
            if (!isSinglePlayer)
            {
                Instantiate(PlayerTwo, P2SpawnPos.position, Quaternion.identity);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(P1SpawnPos.position, Vector2.one);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(P2SpawnPos.position, Vector2.one);
        }
    }

}