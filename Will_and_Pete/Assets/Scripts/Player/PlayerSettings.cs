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
    [Range(0, 10)] [SerializeField] private int doubleJumps;
    [SerializeField] private bool resetDoubleJumpsOnWall;
    [SerializeField] private float fallingSpeedCap;

    [SerializeField] private float wallJumpPower;
    [SerializeField] [PostNormalize] private Vector2 wallJumpDirection;
    [Range(0f, 10f)] [SerializeField] private float wallJumpStunTime;
    [SerializeField] private AnimationCurve wallJumpStunRecoveryCurve;

    [Range(-5, 0)] [SerializeField] private float wallSlideSpeed;
    [Range(0, 10)] [SerializeField] private float wallSlideForce;

    [Range(0, 10)] [SerializeField] private float fallMultiplier;
    [Range(0, 10)] [SerializeField] private float lowJumpMultiplier;
    [Range(0, 10)] [SerializeField] private float airControl;
    [Range(0, 1)] [SerializeField] private float jumpBufferTime;
    [Range(0, 1)] [SerializeField] private float coyoteTime;

    [Header("Shooting")] 
    [SerializeField] private GameObject riflePrefab;
    [SerializeField] private LayerMask shootingLayer;
    [SerializeField] [Range(0,5)] private float fireRate;
    [SerializeField] private float fireRange;
    [SerializeField] [Range(0,50)]  private float knockBackForce;
    [SerializeField] [Range(0,20)] private float aimControlFactor;
    [SerializeField] private float wobbleSpeed;
    [SerializeField] private float wobbleStrength;
    [SerializeField] private AnimationCurve wobbleStrengthCurve;
    [SerializeField] private float upwardsAimThreshold;
    [SerializeField] private float fireLineFadeTime;
    [SerializeField] private Color fireLineStartColor;
    [SerializeField] private Color fireLineEndColor;

    [Header("Throwing")]
    [Range(0,30)] [SerializeField] private float throwForce;
    
    //Misc
    public LayerMask PlayerLayer => playerLayer;
    public float HelpUpTime => helpUpTime;

    #region Movement
    public float Speed => speed;
    public float JumpPower => jumpPower;
    public int DoubleJumps => doubleJumps;
    public bool ResetDoubleJumpsOnWall => resetDoubleJumpsOnWall;
    public float FallingSpeedCap => fallingSpeedCap;

    public float WallJumpPower => wallJumpPower;
    public Vector2 WallJumpDirection => wallJumpDirection.normalized;
    public float WallJumpStunTime => wallJumpStunTime;
    public AnimationCurve WallJumpStunRecoveryCurve => wallJumpStunRecoveryCurve;
    public float WallSlideSpeed => wallSlideSpeed;
    public float WallSlideForce => wallSlideForce;

    public float FallMultiplier => fallMultiplier;
    public float LowJumpMultiplier => lowJumpMultiplier;
    public float AirControl => airControl;
    public float JumpBufferTime => jumpBufferTime;
    public float CoyoteTime => coyoteTime;
    #endregion
    
    #region Shooting
    public GameObject RiflePrefab => riflePrefab;
    public LayerMask ShootingLayer => shootingLayer;
    public float FireRate => fireRate;
    public float FireRange => fireRange;
    public float KnockBackForce => knockBackForce;
    public float AimControlFactor => aimControlFactor;
    public float WobbleSpeed => wobbleSpeed;
    public float WobbleStrength => wobbleStrength;
    public AnimationCurve WobbleStrengthCurve => wobbleStrengthCurve;
    public float UpwardsAimThreshold => upwardsAimThreshold;
    public float FireLineFadeTime => fireLineFadeTime;
    public Color FireLineStartColor => fireLineStartColor;
    public Color FireLineEndColor => fireLineEndColor;
    #endregion
    
    #region Throwing
    //Throwing
    public float ThrowForce => throwForce;
    #endregion
    
}