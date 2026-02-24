using System;
using Assets.Scripts.Player;
using Enemies;
using UnityEngine;
using World;

namespace Player
{
    public class PlayerShooting : MonoBehaviour, IPlayerShooting
    {
        [SerializeField] private LayerMask weaponLayer;
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
            float movementFactor =
                pSettings.WobbleStrengthCurve.Evaluate(rb2d.linearVelocity.magnitude / pSettings.FallingSpeedCap);
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
            float force = 13;
            if (rb2d.linearVelocity.magnitude > 0.5f)
            {
                dir = rb2d.linearVelocity.normalized;
                force += rb2d.linearVelocity.magnitude / 2;
            }
            else
            {
                dir = gunView.GunForwards;
            }

            Vector3 offset = dir * 2;

            GameObject rifle = Instantiate(pSettings.RiflePrefab, transform.position + offset, gunView.AimRotation);
            rifle.GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
            rifle.GetComponent<SpriteRenderer>().flipY = !(gunView.GunForwards.x > 0);
            onWeaponThrown.Invoke(rifle.transform);
            ToggleActive();
        }

        private void Knockback(Vector2 direction, float knockbackForce)
        {
            if (rb2d.linearVelocity.y > 0)
            {
                rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, rb2d.linearVelocity.y / 5);
            }
            else
            {
                rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0);
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

            if (result.transform.TryGetComponent(out IKnockable knockedEntity))
            {
                knockedEntity.Knockback(result.point, transform.position, 8);
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

        private void FixedUpdate()
        {
            CheckForWeapon();
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

        private void CheckForWeapon()
        {
            if (isDisabled)
            {
                Collider2D check = Physics2D.OverlapCircle(transform.position, 0.7f, weaponLayer);
                if (check)
                {
                    onWeaponCollected.Invoke(check.transform);
                    Destroy(check.gameObject);
                    ToggleActive();
                }
            }
        }
    }
}