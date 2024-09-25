using UnityEngine;

public class PlayerShooting : MonoBehaviour, IPlayerShooting
{
    [SerializeField] private ShootingSettings settings;

    public bool ShowGizmos;
    private float fireRateValue;
    private bool canFire;

    private Vector2 lastHitPosition;

    void Update()
    {
        canFire = false;
        canFire = CheckFireRate();
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

    void IPlayerShooting.Fire(Vector2 direction)
    {
        if (!canFire)
        {return;}

        RaycastHit2D result = Physics2D.Raycast(transform.position, direction, settings.fireRange, settings.shootingLayer);

        fireRateValue = 0;
        lastHitPosition = result.point;
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
