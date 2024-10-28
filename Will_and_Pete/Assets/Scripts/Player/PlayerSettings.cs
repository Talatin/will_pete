using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "AvatarSettings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Misc")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float helpUpTime;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [Range(0, 10)][SerializeField] private float fallMultiplier;
    [Range(0, 10)][SerializeField] private float lowJumpMultiplier;
    [Range(0, 10)][SerializeField] private float airControl;
    [Range(0, 1)][SerializeField] private float jumpBufferTime;
    [Range(0, 1)][SerializeField] private float coyoteTime;

    [Header("Shooting")]
    [SerializeField] private LayerMask shootingLayer;
    [SerializeField] private float fireRate;
    [SerializeField] private float fireRange;
    [SerializeField] private float upwardsAimThreshold;
    [SerializeField] private float fireLineFadeTime;
    [SerializeField] private Color fireLineStartColor;
    [SerializeField] private Color fireLineEndColor;

    public LayerMask PlayerLayer { get => playerLayer; private set => playerLayer = value; }
    public float HelpUpTime { get => helpUpTime; private set => helpUpTime = value; }

    public float Speed { get => speed; private set => speed = value; }
    public float JumpPower { get => jumpPower; private set => jumpPower = value; }
    public float FallMultiplier { get => fallMultiplier; private set => fallMultiplier = value; }
    public float LowJumpMultiplier { get => lowJumpMultiplier; private set => lowJumpMultiplier = value; }
    public float AirControl { get => airControl; private set => airControl = value; }
    public float JumpBufferTime { get => jumpBufferTime; private set => jumpBufferTime = value; }
    public float CoyoteTime { get => coyoteTime; private set => coyoteTime = value; }

    public LayerMask ShootingLayer { get => shootingLayer; private  set => shootingLayer = value; }
    public float FireRate { get => fireRate; private  set => fireRate = value; }
    public float FireRange { get => fireRange; private set => fireRange = value; }
    public float UpwardsAimThreshold { get => upwardsAimThreshold; private  set => upwardsAimThreshold = value; }
    public float FireLineFadeTime { get => fireLineFadeTime; private set => fireLineFadeTime = value; }
    public Color FireLineStartColor { get => fireLineStartColor; private set => fireLineStartColor = value; }
    public Color FireLineEndColor { get => fireLineEndColor; private set => fireLineEndColor = value; }
}
