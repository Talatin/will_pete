using System.Collections;
using UnityEngine;
using System;

namespace Assets.Scripts.Enemies
{
    [Serializable]
    public class GroundPatrolSettings
    {
        public bool isDrawingGizmos;
        [Space]
        public Transform ownerTransform;
        public Rigidbody2D ownerRb;
        public enum PatrolType { Walls, Cliffs, Both }
        public DetectPlayer detectPlayer;
        public float speed;
        public PatrolType type;
        public Transform WallCheckTransform;
        public Transform CliffCheckTransform;
        public LayerMask CheckLayer;
        public float CheckRadius;
        public float MaxWaitTime;
    }

    public class GroundPatrolState : EnemyState
    {
        private GroundPatrolSettings settings;
        private bool hasFoundPlayer;
        private bool hasFoundObstacle;
        private float currentWaitTime;

        public GroundPatrolState(GroundPatrolSettings _settings)
        {
            stateName = States.GroundPatrol;
            settings = _settings;
        }

        public override void Enter()
        {

        }

        public override void UpdateState()
        {
            settings.detectPlayer.SearchForPlayer();
            if (hasFoundObstacle)
            {
                WaitTillTurn(settings.ownerTransform);
            }
        }

        public override void FixedUpdateState()
        {
            Move(settings.ownerTransform, settings.ownerRb, settings.speed);
        }
        
        public override void Exit()
        {
            settings.ownerRb.velocity = Vector3.zero;
            settings.ownerRb.AddForce(Vector2.up * 2,ForceMode2D.Impulse);
        }

        public override States CheckExitConditions()
        {
            if (settings.detectPlayer.PlayerTransform != null)
            {
                return States.GroundChase;
            }
            return States.UNCHANGED;
        }

        private void Move(Transform transform, Rigidbody2D rb, float speed)
        {
            if (!hasFoundObstacle)
            {
                if (CheckPath(settings.type))
                {
                    hasFoundObstacle = true;
                    currentWaitTime = 0;
                }
                rb.velocity = new Vector2(transform.localScale.x * speed * Time.deltaTime, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        private bool CheckPath(GroundPatrolSettings.PatrolType patroltype)
        {
            bool result = false;
            switch (patroltype)
            {
                case GroundPatrolSettings.PatrolType.Walls:
                    result = Physics2D.OverlapCircle(settings.WallCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
                    return result;
                case GroundPatrolSettings.PatrolType.Cliffs:
                    result = !Physics2D.OverlapCircle(settings.CliffCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
                    return result;
                case GroundPatrolSettings.PatrolType.Both:
                    result = !Physics2D.OverlapCircle(settings.CliffCheckTransform.position, settings.CheckRadius, settings.CheckLayer) ||
                              Physics2D.OverlapCircle(settings.WallCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
                    return result;
                default:
                    return result;
            }
        }

        private void WaitTillTurn(Transform transform)
        {
            currentWaitTime += Time.deltaTime;
            if (currentWaitTime > settings.MaxWaitTime)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                hasFoundObstacle = false;
            }
        }
    }
}
