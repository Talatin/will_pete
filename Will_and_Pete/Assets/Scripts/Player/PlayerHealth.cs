using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerHealth : MonoBehaviour
    {
        public delegate void TookDamage(bool value);

        public event TookDamage onDownedStateChanged;

        private float reviveTimer = 1f;
        private float currentRevTime = 0;
        private PlayerSettings pSettings;

        public void Initialize(PlayerSettings playerSettings)
        {
            pSettings = playerSettings;
        }

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
            if (collision.CompareTag("Damage"))
            {
                TakeDamage();
            }
        }

        public void HelpUpPlayer()
        {
            Collider2D[] collHits = Physics2D.OverlapCircleAll(transform.position, 2, pSettings.PlayerLayer);
            if (collHits.Length >= 2)
            {
                currentRevTime += Time.deltaTime;
                if (currentRevTime > reviveTimer)
                {
                    foreach (Collider2D coll in collHits)
                    {
                        if (coll.gameObject != this.gameObject)
                        {
                            coll.GetComponent<PlayerHealth>().HelpBackUp();
                        }
                    }
                }
            }
        }
    }
}