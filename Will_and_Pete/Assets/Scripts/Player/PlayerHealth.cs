using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public delegate void TookDamage(bool value);
        public event TookDamage onDownedStateChanged;

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