using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Player
{
    public class PlayerShooting : MonoBehaviour, IPlayerShooting
    {
        private PlayerSettings pSettings;
        private PlayerState pState;
        private GunView gunView;
        private float currentFireRate;
        private bool canFire;
        private bool isDisabled;
        private Rigidbody2D rb2d;

        public void Initialize(PlayerState state, PlayerSettings settings)
        {
            gunView = GetComponent<GunView>();
            pState = state;
            pSettings = settings;
            gunView.Initialize(settings, state);
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            canFire = true;
            currentFireRate = pSettings.FireRate;
        }

        public void Aim(Vector2 direction)
        {
            gunView.RotateToTarget(direction);
        }

        public void ThrowWeapon()
        {
            if (isDisabled)
            {
                return;
            }

            Vector3 dir = gunView.GunForwards;
            Vector3 offset = gunView.GunForwards * 2;

            GameObject rifle = Instantiate(pSettings.RiflePrefab, transform.position + offset, gunView.AimRotation);
            rifle.GetComponent<Rigidbody2D>().AddForce(dir * 18, ForceMode2D.Impulse);
            rifle.GetComponent<SpriteRenderer>().flipY = !(gunView.GunForwards.x > 0);
            ToggleActive();
        }

        public void ToggleActive()
        {
            gunView.ToggleVisibility();
            isDisabled = !isDisabled;
        }

        public bool Fire(Vector2 direction)
        {
            if (isDisabled)
            {
                return false;
            }

            if (!canFire || pState.IsDowned)
            {
                return false;
            }

            currentFireRate = 0;
            RaycastHit2D result = Physics2D.Raycast(transform.position, direction, pSettings.FireRange,
                pSettings.ShootingLayer);
            if (!result.collider)
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
            if (isDisabled)
            {
                return;
            }

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