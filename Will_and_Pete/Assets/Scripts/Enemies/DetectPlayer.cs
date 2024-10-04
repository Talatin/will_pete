using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class DetectPlayer : MonoBehaviour
    {
        public bool DrawGizmos;
        [SerializeField] private Detectionmethod detectionmethod;
        [SerializeField] private float detectionRange;
        [SerializeField] private float detectionCastSize;
        [SerializeField] private Transform detectionTransform;
        [SerializeField] private LayerMask detectionlayer;
        [SerializeField] private float timeToDetect;

        SpriteRenderer spRend;

        private enum Detectionmethod { LookDirection, Radius, AreaEntry }

        private void Awake()
        {
            spRend = GetComponent<SpriteRenderer>();
        }


        public bool SearchForPlayer()
        {
            switch (detectionmethod)
            {
                case Detectionmethod.LookDirection:
                    return Physics2D.CircleCast(detectionTransform.position, detectionCastSize, new Vector2(transform.localScale.x, 0), detectionRange, detectionlayer);
                case Detectionmethod.Radius:
                    return Physics2D.OverlapCircle(transform.position, detectionRange, detectionlayer);
                case Detectionmethod.AreaEntry:
                    return spRend.isVisible;
                default:
                    return false;
            }
        }

        private void OnDrawGizmos()
        {
            if (!DrawGizmos)
            {
                return;
            }

            Gizmos.color = Color.red;
            switch (detectionmethod)
            {
                case Detectionmethod.LookDirection:
                    Vector3 offset = new(0, detectionCastSize / 2, 0);
                    Vector3 target = detectionTransform.position + new Vector3(transform.localScale.x * detectionRange, 0);
                    Gizmos.DrawLine(detectionTransform.position, target);
                    Gizmos.DrawLine(detectionTransform.position + offset, target + offset);
                    Gizmos.DrawLine(detectionTransform.position - offset, target - offset);
                    break;
                case Detectionmethod.Radius:
                    Gizmos.DrawWireSphere(transform.position, detectionRange);
                    break;
                case Detectionmethod.AreaEntry:
                    break;
                default:
                    break;
            }
        }

    }
}