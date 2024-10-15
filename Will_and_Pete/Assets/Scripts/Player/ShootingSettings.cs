using UnityEngine;

namespace Assets.Scripts.Player
{
    [CreateAssetMenu(fileName = "ShootingSettings", menuName = "AvatarSettings/ShootingSettings")]
    public class ShootingSettings : ScriptableObject
    {
        public LayerMask shootingLayer;
        public float fireRate;
        public float fireRange;
        public float upwardsAimThreshold;

        public float fireLineFadeTime;
        public Color fireLineStartColor;
        public Color fireLineEndColor;



    }
}