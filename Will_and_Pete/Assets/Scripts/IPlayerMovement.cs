using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerMovement
{
    public void UpdateMovement(PlayerInput pInput, PlayerState pState);
    public void Jump(PlayerInput pInput = null, PlayerState pState = null);
}
