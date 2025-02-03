using System;
using Unity.VisualScripting;
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
                    }
                    break;
                case Detectionmethod.Radius:
                    Collider2D[] colliderHits = Physics2D.OverlapCircleAll(transform.position, detectionRange, detectionlayer);
                    foreach (Collider2D collider in colliderHits)
                    {
                        if (collider.CompareTag("Player"))
                        {
                            playerTransform = collider.transform;
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

        private void FollowPlayer(Transform followedTransform)
        {
            if (followedTransform != null)
            {
                bool foundPlayer = false;
                var rayTopResult = Physics2D.Raycast(transform.position, ((followedTransform.position + followedTransform.localScale / 2.25f) - transform.position), 15, detectionlayer);
                var rayBottomResult = Physics2D.Raycast(transform.position, ((followedTransform.position - followedTransform.localScale / 2.25f) - transform.position), 15, detectionlayer);
                if (rayTopResult)
                {
                    if (rayTopResult.transform.CompareTag("Player"))
                    {
                        playerTransform = rayTopResult.transform;
                        foundPlayer = true;
                    }
                }
                if (rayBottomResult)
                {
                    if (rayBottomResult.transform.CompareTag("Player"))
                    {
                        playerTransform = rayBottomResult.transform;
                        foundPlayer = true;
                    }
                }
                if (!foundPlayer)
                {
                    playerTransform = null;
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
            Gizmos.color = Color.magenta;
            if (playerTransform)
            {
                Gizmos.DrawRay(transform.position, ((playerTransform.position - playerTransform.localScale / 2.2f) - transform.position));
                Gizmos.DrawRay(transform.position, ((playerTransform.position + playerTransform.localScale / 2.2f) - transform.position));
            }
            
        }

    }
}