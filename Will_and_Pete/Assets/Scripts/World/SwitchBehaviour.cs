using System;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class SwitchBehaviour : MonoBehaviour
    {
        public event Action<bool> OnSwitchStateChanged;
        [SerializeField] private bool stayOn;

        private int amountOfEntitiesOnSwitch = -1;
        private bool State => entitiesOnSwitch.Count > 0;
        private List<GameObject> entitiesOnSwitch = new List<GameObject>();
    
        private Animator animator;
        private readonly int switchHashOn = Animator.StringToHash("On");
        private readonly int switchHashOff = Animator.StringToHash("Off");

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            amountOfEntitiesOnSwitch++;
            if (other.CompareTag("Player"))
            {
                if (!entitiesOnSwitch.Contains(other.gameObject))
                {
                    entitiesOnSwitch.Add(other.gameObject);
                }
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
                if (entitiesOnSwitch.Contains(other.gameObject))
                {
                    entitiesOnSwitch.Remove(other.gameObject);
                }
                OnSwitchStateChanged?.Invoke(State);
                animator.SetTrigger( State ? switchHashOn : switchHashOff);
            }
        }
    }
}
