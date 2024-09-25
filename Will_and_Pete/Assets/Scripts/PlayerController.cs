using UnityEngine;

public class PlayerController : MonoBehaviour
{
    IPlayerMovement playerMovement;
    IPlayerShooting playerShooting;
    PlayerInput playerInput;
    PlayerState playerState;

    private void Awake()
    {
        playerState = GetComponent<PlayerState>();
        playerInput = GetComponent<PlayerInput>();
        playerShooting = GetComponent<IPlayerShooting>();
        playerMovement = GetComponent<IPlayerMovement>();
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

        #region deprecated
        if (playerInput.BackPackInput)
        {
            //deprecated
            playerInput.BackPackInput = false;
        }
        if (playerInput.BackPackHeld)
        {
            //deprecated
            playerInput.BackPackHeld = false;
        }
        if (playerInput.AbilityOneInput)
        {
            //deprecated
            playerInput.AbilityOneInput = false;
        }
        if (playerInput.AbilityTwoInput)
        {
            //deprecated
            playerInput.AbilityTwoInput = false;
        }
        if (playerInput.AimingInput != Vector2.zero)
        {
            //deprecated
        }
        #endregion

        if (playerInput.FireInput)
        {
            Vector2 aimDirection = playerState.isFacingRight ? Vector2.right : Vector2.left;
            playerShooting.Fire(aimDirection);
            playerInput.FireInput = false;
        }
        if (playerInput.CrouchInput)
        {
            Debug.Log($"{gameObject.name} used crouch");
        }
    }

    private void HandleFixedUpdateInputs()
    {
        playerMovement.UpdateMovement(playerInput, playerState);
    }
}
