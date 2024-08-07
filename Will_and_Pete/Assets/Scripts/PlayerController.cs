using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerMovement playerMovement;
    PlayerInput playerInput;
    PlayerState playerState;


    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<IPlayerMovement>();
    }
  
    private void Update()
    {
        if (playerInput.JumpInput)
        {
            playerMovement.Jump(playerInput, playerState);
            playerInput.JumpInput = false;
        }
    }
    private void FixedUpdate()
    {
        playerMovement.UpdateMovement(playerInput, playerState);
    }
}
