using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "AvatarSettings/PlayerSettings")]
public class PlayerSettings : ScriptableObject
{
    private const string CHEATING_MAP_NAME = "Cheating";
    [Header("Misc")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float helpUpTime;
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [Range(0, 10)][SerializeField] private int doubleJumps;

    [SerializeField] private float wallJumpPower;
    [SerializeField][PostNormalize] private Vector2 wallJumpDirection;
    [Range(0f, 10f)][SerializeField] private float wallJumpStunTime;
    [SerializeField] private AnimationCurve wallJumpStunRecoveryCurve;

    [Range(-5, 0)][SerializeField] private float wallSlideSpeed;
    [Range(0, 10)][SerializeField] private float wallSlideForce;

    [Range(0, 10)][SerializeField] private float fallMultiplier;
    [Range(0, 10)][SerializeField] private float lowJumpMultiplier;
    [Range(0, 10)][SerializeField] private float airControl;
    [Range(0, 1)][SerializeField] private float jumpBufferTime;
    [Range(0, 1)][SerializeField] private float coyoteTime;

    [Header("Shooting")] 
    [SerializeField] private GameObject riflePrefab;
    [SerializeField] private LayerMask shootingLayer;
    [SerializeField] private float fireRate;
    [SerializeField] private float fireRange;
    [SerializeField] private float upwardsAimThreshold;
    [SerializeField] private float fireLineFadeTime;
    [SerializeField] private Color fireLineStartColor;
    [SerializeField] private Color fireLineEndColor;
    //Misc
    public LayerMask PlayerLayer { get => playerLayer; private set => playerLayer = value; }
    public float HelpUpTime { get => helpUpTime; private set => helpUpTime = value; }
    //Movement
    public float Speed { get => speed; private set => speed = value; }
    public float JumpPower { get => jumpPower; private set => jumpPower = value; }
    public int DoubleJumps { get => doubleJumps; private set => doubleJumps = value; }

    public float WallJumpPower { get => wallJumpPower; private set => wallJumpPower = value; }
    public Vector2 WallJumpDirection { get => wallJumpDirection.normalized; private set => wallJumpDirection = value; }
    public float WallJumpStunTime { get => wallJumpStunTime; private set => wallJumpStunTime = value; }
    public AnimationCurve WallJumpStunRecoveryCurve { get => wallJumpStunRecoveryCurve; private set => wallJumpStunRecoveryCurve = value; }
    public float WallSlideSpeed { get => wallSlideSpeed; private set => wallSlideSpeed = value; }
    public float WallSlideForce { get => wallSlideForce; private set => wallSlideForce = value; }

    public float FallMultiplier { get => fallMultiplier; private set => fallMultiplier = value; }
    public float LowJumpMultiplier { get => lowJumpMultiplier; private set => lowJumpMultiplier = value; }
    public float AirControl { get => airControl; private set => airControl = value; }
    public float JumpBufferTime { get => jumpBufferTime; private set => jumpBufferTime = value; }
    public float CoyoteTime { get => coyoteTime; private set => coyoteTime = value; }
    //Shooting
    public GameObject RiflePrefab { get => riflePrefab; private set => riflePrefab = value; }
    public LayerMask ShootingLayer { get => shootingLayer; private set => shootingLayer = value; }
    public float FireRate { get => fireRate; private set => fireRate = value; }
    public float FireRange { get => fireRange; private set => fireRange = value; }
    public float UpwardsAimThreshold { get => upwardsAimThreshold; private set => upwardsAimThreshold = value; }
    public float FireLineFadeTime { get => fireLineFadeTime; private set => fireLineFadeTime = value; }
    public Color FireLineStartColor { get => fireLineStartColor; private set => fireLineStartColor = value; }
    public Color FireLineEndColor { get => fireLineEndColor; private set => fireLineEndColor = value; }
}
