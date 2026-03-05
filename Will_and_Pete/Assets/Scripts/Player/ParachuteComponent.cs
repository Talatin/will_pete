using System;
using Assets.Scripts.Player;
using UnityEngine;

namespace Player
{
    public class ParachuteComponent : MonoBehaviour
    {
        private float fallTime;
        private PlayerState playerState;
        private Rigidbody2D rb;
        private int myPlayerID;
        [SerializeField] private float fallTimeThreshold;
        [SerializeField] private float parachuteFallSpeed;
        [SerializeField] private float parachuteBreakStrength;
        [SerializeField] private GameObject ParachuteObject;
        private bool isNoClipping = false;

        public void Initialize(PlayerController controller)
        {
            myPlayerID = controller.playerID;
            playerState = controller.PlayerState;
            rb = controller.rb;
            CheatSystem.OnNoclipToggled += ToggleNoclip;
        }

        private void ToggleNoclip(int id)
        {
            if (myPlayerID == id)
            {
                isNoClipping = !isNoClipping;
            }
        }

        private void FixedUpdate()
        {
            if (!isNoClipping)
            {
                HandleAutomaticParachute();
            }
        }

        private void HandleAutomaticParachute()
        {
            if (playerState.IsGrounded || playerState.IsWalledLeft || playerState.IsWalledRight || rb.linearVelocity.y > Mathf.Epsilon)
            {
                fallTime = 0;
                if (ParachuteObject.activeSelf)
                {
                    ParachuteObject.SetActive(false);
                }
                return;
            }
            
            fallTime += Time.deltaTime;
            if (fallTime > fallTimeThreshold)
            {
                if (!ParachuteObject.activeSelf)
                {
                    ParachuteObject.SetActive(true);
                }

                RotateParachute();
                rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, new Vector2(rb.linearVelocity.x, -parachuteFallSpeed), parachuteBreakStrength * Time.deltaTime);
            }
        }
        
        private void RotateParachute()
        {
            ParachuteObject.transform.up = transform.up + new Vector3(-rb.linearVelocity.x, 20, 0);
        }
    }
}