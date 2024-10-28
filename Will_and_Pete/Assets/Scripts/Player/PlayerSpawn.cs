using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        [SerializeField] private Transform P1SpawnPos;
        [SerializeField] private Transform P2SpawnPos;
        [SerializeField] private CinemachineTargetGroup TargetGroup;
        private bool noPlayerSpawned = true;
        private PlayerInputManager playerInputManager;

        // Start is called before the first frame update
        private void Start()
        {
            playerInputManager = GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += OnPlayerJoined;
        }

        private void OnPlayerJoined(UnityEngine.InputSystem.PlayerInput input)
        {
            TargetGroup.AddMember(input.transform, 1, 3);
            if (noPlayerSpawned)
            {
                input.transform.position = P1SpawnPos.position;
                noPlayerSpawned = false;
            }
            else
            {
                input.transform.position = P2SpawnPos.position;
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