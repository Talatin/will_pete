using UnityEngine;

public class PlayerShooting : MonoBehaviour, IPlayerShooting
{
    [SerializeField] private ShootingSettings settings;

    public bool ShowGizmos;
    private float fireRateValue;
    private bool canFire;

    private Vector2 lastHitPosition;
    private void Update()
    {
        canFire = false;
        canFire = CheckFireRate();
    }

    public bool Fire(Vector2 direction)
    {
        if (!canFire)
        { return false; }

        fireRateValue = 0;
        RaycastHit2D result = Physics2D.Raycast(transform.position, direction, settings.fireRange, settings.shootingLayer);

        if (result.collider == null)
        { return true; }

        lastHitPosition = result.point;

        if (result.transform.TryGetComponent<IDamageable>(out IDamageable damagedEntity))
        {
            damagedEntity.TakeDamage();
        }
        return true;
    }

    private bool CheckFireRate()
    {
        if (fireRateValue < settings.fireRate)
        {
            fireRateValue += Time.deltaTime;
            return false;
        }
        else
        {
            fireRateValue = settings.fireRate;
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        if (ShowGizmos && lastHitPosition != Vector2.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lastHitPosition, 0.25f);
        }
    }
}
