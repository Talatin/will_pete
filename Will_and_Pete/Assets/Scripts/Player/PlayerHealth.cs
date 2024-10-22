using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public enum DamageType { Health, Knockback, InstaKill, reset }
        public bool isDowned {  get; private set; }
        private Vector3 lastStandingPosition;
        public Vector3 LastStandingPosition { set { lastStandingPosition = value; } }
       
        public void TakeDamage(DamageType type, Transform source)
        {
            isDowned = true;
        }

        public void HelpBackUp()
        {
            isDowned = false;
        }
    }
}