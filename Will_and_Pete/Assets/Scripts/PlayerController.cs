using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerMovement playerMovement;
    PlayerInput playerInput;
    PlayerState playerState;


    private void Awake()
    {
        GetComponents();
    }

    private void Update()
    {
        HandleUpdateInputs();
    }

    private void FixedUpdate()
    {
        HandleFixedUpdateInputs();
    }

    private void HandleUpdateInputs()
    {
        if (playerInput.JumpInput)
        {
            playerMovement.Jump(playerInput, playerState);
            playerInput.JumpInput = false;
        }

        if (playerInput.BackPackInput)
        {
            Debug.Log($"{gameObject.name} used Backpack");
            playerInput.BackPackInput = false;
        }
        if (playerInput.BackPackHeld)
        {
            Debug.Log($"{gameObject.name} held Backpack");
            playerInput.BackPackHeld = false;
        }
        if (playerInput.AbilityOneInput)
        {
            Debug.Log($"{gameObject.name} used ability 1");
            playerInput.AbilityOneInput = false;
        }
        if (playerInput.AbilityTwoInput)
        {
            Debug.Log($"{gameObject.name} used ability 2");
            playerInput.AbilityTwoInput = false;
        }
    }

    private void HandleFixedUpdateInputs()
    {
        playerMovement.UpdateMovement(playerInput, playerState);
    }

    private void GetComponents()
    {
        playerState = GetComponent<PlayerState>();
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<IPlayerMovement>();
    }
}
