using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace World
{
    public class SwitchBehaviour : MonoBehaviour
    {
        [SerializeField] private bool stayOn;
        [SerializeField] private List<AActivatable> switchListeners;
        [SerializeField] private LayerMask interactLayer;
        [SerializeField] private Animator animator;
        

        private bool State => entitiesOnSwitch.Count > 0;
        private List<GameObject> entitiesOnSwitch = new List<GameObject>();
    
        private readonly int switchHashOn = Animator.StringToHash("On");
        private readonly int switchHashOff = Animator.StringToHash("Off");

        private void ToggleListeners(bool state)
        {
            foreach (AActivatable listener in switchListeners)
            {
                if (state)
                {
                    listener.Activate();
                }
                else
                {
                    listener.Deactivate();
                }
            }
        }
        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (interactLayer == (interactLayer | (1 << other.gameObject.layer)))
            {
                if (!entitiesOnSwitch.Contains(other.gameObject))
                {
                    entitiesOnSwitch.Add(other.gameObject);
                }
                ToggleListeners(State);
                animator.SetTrigger( State ? switchHashOn : switchHashOff);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (stayOn)
            {
                return;
            }
            if (interactLayer == (interactLayer | (1 << other.gameObject.layer)))
            {
                if (entitiesOnSwitch.Contains(other.gameObject))
                {
                    entitiesOnSwitch.Remove(other.gameObject);
                }
                ToggleListeners(State);
                animator.SetTrigger( State ? switchHashOn : switchHashOff);
            }
        }
    }
}
