using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class DetectPlayer : MonoBehaviour
    {

        public delegate void OnPlayerFound(Transform playerTransform);
        public event OnPlayerFound onPlayerFound;

        public bool DrawGizmos;
        [SerializeField] private Detectionmethod detectionmethod;
        [SerializeField] private float detectionRange;
        [SerializeField] private float detectionCastSize;
        [SerializeField] private Transform detectionTransform;
        [SerializeField] private LayerMask detectionlayer;
        //[SerializeField] private float timeToDetect;
        private Transform playerTransform;
        public Transform PlayerTransform { get { return playerTransform; } }
        private bool hasFoundPlayer { get { return playerTransform != null; } }

        private enum Detectionmethod { LookDirection, Radius }

        public void SearchForPlayer()
        {
            switch (detectionmethod)
            {
                case Detectionmethod.LookDirection:
                    RaycastHit2D raycastHit = Physics2D.Raycast(detectionTransform.position, new Vector2(transform.localScale.x, 0), detectionRange, detectionlayer);
                    if (raycastHit.collider == null)
                    { return; }
                    if (raycastHit.transform.CompareTag("Player"))
                    {
                        playerTransform = raycastHit.transform;
                        onPlayerFound?.Invoke(raycastHit.transform);
                    }
                    break;
                case Detectionmethod.Radius:
                    Collider2D[] colliderHits = Physics2D.OverlapCircleAll(transform.position, detectionRange, detectionlayer);
                    foreach (Collider2D collider in colliderHits)
                    {
                        if (collider.CompareTag("Player"))
                        {
                            playerTransform = collider.transform;
                            onPlayerFound?.Invoke(collider.transform);
                        }
                    }
                    break;
                default:
                    throw new System.Exception($"{gameObject.name} Unknown State in DetectionMethod");
            }
        }

        private void FixedUpdate()
        {
            if (!hasFoundPlayer)
            {
                SearchForPlayer(); 
            }
            else
            {
                FollowPlayer(playerTransform);
            }
        }

        private void FollowPlayer(Transform playerTransform)
        {
            if (playerTransform != null)
            {
                var rayResult = Physics2D.Raycast(transform.position, (playerTransform.position - transform.position), 10, detectionlayer);
                if (rayResult)
                {
                    if (rayResult.transform.CompareTag("Player"))
                    {
                        playerTransform = rayResult.transform;
                    }
                    else
                    {
                        playerTransform = null;

                    }
                }
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