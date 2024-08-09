using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour, IPlayerMovement
{
    [SerializeField] private MovementSettings settings;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void UpdateMovement(PlayerInput pInput, PlayerState pState)
    {
        Move(pInput, pState);
        GravityManipulation(pInput);
    }

    public void Jump(PlayerInput pInput, PlayerState pState)
    {
        if (!pState.isGrounded)
        {
            return;
        }
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * settings.jumpPower, ForceMode2D.Impulse);
    }

    private void Move(PlayerInput pInput, PlayerState pState)
    {
        if (pState.isGrounded) //Ground Movement = Snappy and fast
        {
            rb.velocity = new Vector2(pInput.MovementInput.x * settings.speed * Time.deltaTime, rb.velocity.y);
        }
        else //Air movement = Takes more time to reach same speed or stop | Change airControl to adjust the effect
        {
            rb.velocity = Vector2.Lerp(rb.velocity, new Vector2(pInput.MovementInput.x * settings.speed * Time.fixedDeltaTime, rb.velocity.y), settings.airControl * Time.deltaTime);
        }
    }

    private void GravityManipulation(PlayerInput pInput)
    { // Set Charactergravity according to current y velocity and jump input
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = settings.fallMultiplier;
        }
        if (rb.velocity.y > 0 && !pInput.JumpInput)
        {
            rb.gravityScale = settings.lowJumpMultiplier;
        }
        else if (rb.velocity.y >= 0)
        {
            rb.gravityScale = 1;
        }
    }


}
