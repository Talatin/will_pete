using Cinemachine;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private GameObject PlayerTwo;
        [SerializeField] private GameObject PlayerOne;

        [SerializeField] private Transform P1SpawnPos;
        [SerializeField] private Transform P2SpawnPos;
        [SerializeField] private CinemachineTargetGroup TargetGroup;

        public bool isSinglePlayer;
        // Start is called before the first frame update
        void Start()
        {
            var a = Instantiate(PlayerOne, P1SpawnPos.position, Quaternion.identity);
            TargetGroup.AddMember(a.transform, 1, 3);
            if (!isSinglePlayer)
            {
                var b = Instantiate(PlayerTwo, P2SpawnPos.position, Quaternion.identity);
                TargetGroup.AddMember(b.transform, 1, 3);
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