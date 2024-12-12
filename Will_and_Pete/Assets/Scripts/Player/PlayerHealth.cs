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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Damage"))
            {
                TakeDamage();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Damage"))
            {
                TakeDamage();
            }
        }

    }
}