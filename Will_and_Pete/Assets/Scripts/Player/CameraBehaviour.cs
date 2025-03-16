using Cinemachine;
using UnityEngine;

namespace Player
{
    public class CameraBehaviour : MonoBehaviour
    {
        
        private CinemachineTargetGroup cinemachineTargetGroup;

        private void Awake()
        {
            cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        }

        public void AddTransformToGroup(Transform targetTransform)
        {
            cinemachineTargetGroup.AddMember(targetTransform,1,3);
        }

        public void RemoveTransformFromGroup(Transform targetTransform)
        {
            cinemachineTargetGroup.RemoveMember(targetTransform);
        }
    }
}