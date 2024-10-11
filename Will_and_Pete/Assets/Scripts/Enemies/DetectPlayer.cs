using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class DetectPlayer : MonoBehaviour
    {

        public delegate void OnPlayerFound(Vector3 pos);
        public event OnPlayerFound onPlayerFound;

        public bool DrawGizmos;
        [SerializeField] private Detectionmethod detectionmethod;
        [SerializeField] private float detectionRange;
        [SerializeField] private float detectionCastSize;
        [SerializeField] private Transform detectionTransform;
        [SerializeField] private LayerMask detectionlayer;
        //[SerializeField] private float timeToDetect;

        private SpriteRenderer spRend;

        private enum Detectionmethod { LookDirection, Radius }

        private void Awake()
        {
            spRend = GetComponent<SpriteRenderer>();
        }

        public void SearchForPlayer()
        {
            switch (detectionmethod)
            {
                case Detectionmethod.LookDirection:
                    RaycastHit2D raycastHit = Physics2D.Raycast(detectionTransform.position, new Vector2(transform.localScale.x, 0), detectionRange, detectionlayer);
                    if (raycastHit.transform.CompareTag("Player"))
                    {
                        onPlayerFound?.Invoke(raycastHit.transform.position);
                    }
                    break;
                case Detectionmethod.Radius:
                    Collider2D[] colliderHits = Physics2D.OverlapCircleAll(transform.position, detectionRange, detectionlayer);
                    foreach (Collider2D collider in colliderHits)
                    {
                        if (collider.CompareTag("Player"))
                        {
                            onPlayerFound?.Invoke(collider.transform.position);
                        }
                    }
                    break;
                default:
                    throw new System.Exception($"{gameObject.name} Unknown State in DetectionMethod");
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
                    Gizmos.DrawLine(detectionTransform.position, detectionTransform.position + new Vector3(transform.localScale.x * detectionRange, 0));
                    break;
                case Detectionmethod.Radius:
                    Gizmos.DrawWireSphere(transform.position, detectionRange);
                    break;
                default:
                    break;
            }
        }

    }
}