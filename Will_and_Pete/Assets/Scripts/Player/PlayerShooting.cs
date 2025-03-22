using System;
using Player;
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
        private event Action<Transform> onWeaponThrown; 
        private event Action<Transform> onWeaponCollected;
        
        public void Initialize(PlayerState state, PlayerSettings settings, CameraBehaviour cameraBehaviour)
        {
            gunView = GetComponent<GunView>();
            pState = state;
            pSettings = settings;
            gunView.Initialize(settings, state);
            rb2d = GetComponent<Rigidbody2D>();
            onWeaponCollected += cameraBehaviour.RemoveTransformFromGroup;
            onWeaponThrown += cameraBehaviour.AddTransformToGroup;
            ToggleActive();
            
        }

        private void OnEnable()
        {
            canFire = true;
            currentFireRate = pSettings.FireRate;
        }
        
        public void Aim(Vector2 direction)
        {
            if (!pState.IsKneeling || !pState.IsGrounded)
            {
                Vector2 offset = new Vector2(0f, 0.05f);
                gunView.RotateToTarget(pState.IsFacingRight ? Vector2.right + offset : Vector2.left + offset);
                return;
            }
            Vector2 aimOffsetWobble = Vector2.Perpendicular(direction);
            float movementFactor = pSettings.WobbleStrengthCurve.Evaluate(rb2d.velocity.magnitude / pSettings.FallingSpeedCap);
            aimOffsetWobble *= Mathf.Sin(Time.time * pSettings.WobbleSpeed * movementFactor) *
                               pSettings.WobbleStrength * movementFactor;
            gunView.RotateToTarget(direction + aimOffsetWobble);
            gunView.DrawAimLine(gunView.GunForwards * 5);
        }

        public void ThrowWeapon()
        {
            if (isDisabled)
            {
                return;
            }

            Vector3 dir = pState.IsFacingRight ? Vector3.right : Vector3.left;
            Vector3 offset = dir * 2;

            GameObject rifle = Instantiate(pSettings.RiflePrefab, transform.position + offset, gunView.AimRotation);
            rifle.GetComponent<Rigidbody2D>().AddForce(dir * 18, ForceMode2D.Impulse);
            rifle.GetComponent<SpriteRenderer>().flipY = !(gunView.GunForwards.x > 0);
            onWeaponThrown.Invoke(rifle.transform);
            ToggleActive();
        }

        private void Knockback(Vector2 direction, float knockbackForce)
        {
            if (rb2d.velocity.y > 0)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y / 5);
            }
            else
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
            }
            rb2d.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }

        public void ToggleActive()
        {
            gunView.ToggleVisibility();
            isDisabled = !isDisabled;
        }

        public bool Fire(Vector2 direction)
        {
            if (isDisabled || !pState.IsKneeling || !pState.IsGrounded || !canFire || pState.IsDowned)
            {
                return false;
            }

            currentFireRate = 0;
            RaycastHit2D result = Physics2D.Raycast(transform.position, gunView.GunForwards, pSettings.FireRange,
                pSettings.ShootingLayer);
            if (!result.collider)
            {
                gunView.DrawFireLine(transform.position + (Vector3)gunView.GunForwards * 100);
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
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Rifle"))
            {
                onWeaponCollected.Invoke(other.transform);
                Destroy(other.gameObject);
                ToggleActive();
            }
        }
        
    }
}