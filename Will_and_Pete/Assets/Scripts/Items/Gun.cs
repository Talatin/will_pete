using Assets.Scripts.Player;
using Enemies;
using UnityEngine;
using World;

namespace Items
{
    public class Gun : Item
    {
        [SerializeField] private Transform gunTurnAxis;
        [SerializeField] private GameObject gunFireAnimPrefab;
        [SerializeField] private Transform gunNozzlePosition;
        [SerializeField] private LineRenderer lineRendererFire;
        [SerializeField] private LineRenderer lineRendererAim;
        [SerializeField] private SpriteRenderer gunSpriteRenderer;

        private GameObject gunFireAnim;
        private float currentLineFadeTime;

        private float currentFireRate;
        private bool canFire;
        private bool isDisabled => Player is null;
        private bool isAiming;

        protected override void Awake()
        {
            base.Awake();
            SpriteRend = gunSpriteRenderer;
            gunFireAnim = Instantiate(gunFireAnimPrefab);
            gunFireAnim.transform.parent = transform;
        }
        
        private void Update()
        {
            if (isDisabled)
            {
                return;
            }

            canFire = false;
            canFire = CheckFireRate();
            lineRendererAim.enabled = isAiming;
            Aim(Player.PlayerInput.AimingInput);
            FadeFireLine();
        }
        
        protected override void OnRemoveFromPlayer()
        {
            base.OnRemoveFromPlayer();
            Player.PlayerInput.FireEvent -= TryFire;
            currentLineFadeTime = Player.PlayerSettings.FireLineFadeTime;
        }
        
        protected override void ChangeState(bool isOnGround)
        {
            lineRendererAim.enabled = !isOnGround;
            lineRendererFire.enabled = !isOnGround;
            base.ChangeState(isOnGround);
        }

        public override Item Pickup(PlayerController player)
        {
            player.PlayerInput.FireEvent += TryFire;
            return base.Pickup(player);
        }
        
        private void TryFire()
        {
            Fire(Player.PlayerInput.AimingInput);
        }

        private void Aim(Vector2 direction)
        {
            if (isDisabled)
            {
                Vector2 offset = new Vector2(0f, 0.05f);
                //gunView.RotateToTarget(owner.PlayerState.IsFacingRight ? Vector2.right + offset : Vector2.left + offset);
                return;
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, angle),
                Time.deltaTime * Player.PlayerSettings.AimControlFactor);
            // gunTurnAxis.rotation = Quaternion.Euler(0f, 0f, angle);
            SpriteRend.flipY = !(direction.x > 0);

            DrawAimLine(transform.right * 5);
        }

        private void Fire(Vector2 direction)
        {
            if (isDisabled || !canFire || Player.PlayerState.IsDowned)
            {
                return;
            }

            currentFireRate = 0;
            RaycastHit2D result = Physics2D.Raycast(transform.position, transform.right,
                Player.PlayerSettings.FireRange,
                Player.PlayerSettings.ShootingLayer);
            if (!result.collider)
            {
                DrawFireLine(transform.position + (Vector3)transform.right * 100);
                return;
            }

            DrawFireLine(result.point);

            if (result.transform.TryGetComponent(out IDamageable damagedEntity))
            {
                damagedEntity.TakeDamage();
            }

            if (result.transform.TryGetComponent(out IKnockable knockedEntity))
            {
                knockedEntity.Knockback(result.point, transform.position, 8);
            }
        }

        private bool CheckFireRate()
        {
            if (currentFireRate < Player.PlayerSettings.FireRate)
            {
                currentFireRate += Time.deltaTime;
                return false;
            }
            else
            {
                currentFireRate = Player.PlayerSettings.FireRate;
                return true;
            }
        }

        private void FadeFireLine()
        {
            if (currentLineFadeTime >= Player.PlayerSettings.FireLineFadeTime)
            {
                return;
            }

            currentLineFadeTime += Time.deltaTime;
            Color lineColor = Color.Lerp(Player.PlayerSettings.FireLineStartColor,
                Player.PlayerSettings.FireLineEndColor, currentLineFadeTime / Player.PlayerSettings.FireLineFadeTime);
            lineRendererFire.startColor = lineColor;
            lineRendererFire.endColor = lineColor;
        }

        private void DrawFireLine(Vector2 endPos)
        {
            Vector3[] linePositions = { gunNozzlePosition.position, (Vector3)endPos };
            lineRendererFire.SetPositions(linePositions);
            currentLineFadeTime = 0;

            gunFireAnim.transform.localPosition = gunNozzlePosition.position;
            gunFireAnim.transform.rotation = gunTurnAxis.rotation;
            gunFireAnim.SetActive(true);
        }

        private void DrawAimLine(Vector2 endPos)
        {
            Vector3[] linePositions = { gunNozzlePosition.position, gunNozzlePosition.position + (Vector3)endPos };
            lineRendererAim.SetPositions(linePositions);
            isAiming = true;
        }
    }
}