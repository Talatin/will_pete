

using Assets.Scripts.Enemies;
using UnityEngine;
using UnityEngine.Windows;

internal class PatrolStrategy : MonoBehaviour, IEnemyMoveStrategy
{
    private enum PatrolType { Walls, Cliffs, Both }
    [SerializeField] private PatrolType patroltype;
    [SerializeField] private Transform wallCheckPos;
    [SerializeField] private Transform cliffCheckPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask checkLayer;
    
    [SerializeField] float speed;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move()
    {
        if (CheckPath())
        {
            transform.localScale = new Vector3(transform.localScale.x  * -1, 1, 1);
        }
        rb.velocity = new Vector2(transform.localScale.x * speed * Time.deltaTime, rb.velocity.y);
    }

    private bool CheckPath()
    {
        switch (patroltype)
        {
            case PatrolType.Walls:
                return Physics2D.OverlapCircle(wallCheckPos.position, checkRadius, checkLayer);
            case PatrolType.Cliffs:
                return !Physics2D.OverlapCircle(cliffCheckPos.position, checkRadius, checkLayer);
            case PatrolType.Both:
                return !Physics2D.OverlapCircle(cliffCheckPos.position, checkRadius, checkLayer) ||
                        Physics2D.OverlapCircle(wallCheckPos.position, checkRadius, checkLayer);
            default:
                return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(wallCheckPos.position, checkRadius);
        Gizmos.DrawWireSphere(cliffCheckPos.position, checkRadius);
    }


}
