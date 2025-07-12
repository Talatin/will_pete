using System;
using UnityEngine;

namespace World
{
    public class SwitchBehaviour : MonoBehaviour
    {
        public event Action<bool> OnSwitchStateChanged;
        [SerializeField] private bool stayOn;

        private int amountOfEntitiesOnSwitch = -1;
        private bool State => amountOfEntitiesOnSwitch > 0;
    
        private Animator animator;
        private int switchHashOn = Animator.StringToHash("On");
        private int switchHashOff = Animator.StringToHash("Off");

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            amountOfEntitiesOnSwitch++;
            if (other.CompareTag("Player"))
            {
                OnSwitchStateChanged?.Invoke(State);
                animator.SetTrigger( State ? switchHashOn : switchHashOff);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (stayOn)
            {
                return;
            }
            amountOfEntitiesOnSwitch--;
            if (other.CompareTag("Player"))
            {
                OnSwitchStateChanged?.Invoke(State);
                animator.SetTrigger( State ? switchHashOn : switchHashOff);
            }
        }
    }
}
