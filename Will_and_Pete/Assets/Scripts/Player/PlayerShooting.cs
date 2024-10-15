using UnityEngine;

namespace Assets.Scripts.Player
{
    public class PlayerShooting : MonoBehaviour, IPlayerShooting
    {
        [SerializeField] private ShootingSettings settings;

        private GunView gunView;
        private float currentFireRate;
        private bool canFire;

        private void Awake()
        {
            gunView = GetComponent<GunView>();
        }

        private void Update()
        {
            canFire = false;
            canFire = CheckFireRate();
        }

        public void Aim(PlayerState pState, Vector2 direction)
        {
            gunView.RotateToTarget(pState,direction);
        }

        public bool Fire(Vector2 direction)
        {
            if (!canFire)
            { return false; }

            currentFireRate = 0;
            RaycastHit2D result = Physics2D.Raycast(transform.position, direction, settings.fireRange, settings.shootingLayer);
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

        private bool CheckFireRate()
        {
            if (currentFireRate < settings.fireRate)
            {
                currentFireRate += Time.deltaTime;
                return false;
            }
            else
            {
                currentFireRate = settings.fireRate;
                return true;
            }
        }
    }
}