using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerShooting : MonoBehaviour, IPlayerShooting
    {
        private PlayerSettings pSettings;
        private PlayerState pState;
        private GunView gunView;
        private float currentFireRate;
        private bool canFire;

        public void Initialize(PlayerState state, PlayerSettings settings)
        {
            gunView = GetComponent<GunView>();
            pState = state;
            pSettings = settings;
            gunView.Initialize(settings, state);
        }

        public void Aim(Vector2 direction)
        {
            gunView.RotateToTarget(direction);
        }

        public bool Fire(Vector2 direction)
        {
            if (!canFire || pState.IsDowned)
            { return false; }

            currentFireRate = 0;
            RaycastHit2D result = Physics2D.Raycast(transform.position, direction, pSettings.FireRange, pSettings.ShootingLayer);
            if (result.collider == null)
            {
                gunView.DrawFireLine(transform.position + (Vector3)direction * 100);
                return true;
            }

            gunView.DrawFireLine(result.point);

            if (result.transform.TryGetComponent(out IDamageable damagedEntity))
            {
                damagedEntity.TakeDamage();
            }
            return true;
        }

        private void Update()
        {
            canFire = false;
            canFire = CheckFireRate();
        }

        private bool CheckFireRate()
        {
            if (currentFireRate < pSettings.FireRate)
            {
                currentFireRate += Time.deltaTime;
                return false;
            }
            else
            {
                currentFireRate = pSettings.FireRate;
                return true;
            }
        }


    }
}