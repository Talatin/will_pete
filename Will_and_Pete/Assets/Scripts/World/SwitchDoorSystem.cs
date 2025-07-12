using UnityEngine;

namespace World
{
    public class SwitchDoorSystem : MonoBehaviour
    {
        [SerializeField] private Animator doorAnimator;
        [SerializeField] private SwitchBehaviour switchBehaviour;
        [SerializeField] private GameObject doorGameObject;

    
        private void Start()
        {
            switchBehaviour.OnSwitchStateChanged += ToggleDoor;
        }

        private void ToggleDoor(bool state)
        {
            doorGameObject.SetActive(!state);
        }
    }
}
