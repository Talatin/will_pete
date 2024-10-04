using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "AvatarSettings/MovementSettings")]
public class MovementSettings : ScriptableObject
{
    public float speed;
    public float jumpPower;
    [Range(0, 10)] public float fallMultiplier;
    [Range(0, 10)] public float lowJumpMultiplier;
    [Range(0,10)] public float airControl;
    [Range(0, 1)] public float jumpBufferTime;
    [Range(0, 1)] public float coyoteTime;

}
