using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public delegate void TookDamage(bool value);
        public event TookDamage onDownedStateChanged;
        private Vector3 lastStandingPosition;
        public Vector3 LastStandingPosition { set { lastStandingPosition = value; } }

        public void TakeDamage()
        {
            onDownedStateChanged?.Invoke(true);
        }

        public void HelpBackUp()
        {
            onDownedStateChanged?.Invoke(false);
        }
    }
}