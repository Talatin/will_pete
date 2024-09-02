using UnityEngine;

[CreateAssetMenu(fileName = "MovementSettings", menuName = "AvatarSettings")]
public class MovementSettings : ScriptableObject
{
    public float speed;
    public float jumpPower;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public float airControl;

}
