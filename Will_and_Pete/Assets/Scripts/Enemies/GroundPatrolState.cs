using System;
using UnityEngine;

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
        public Transform TargetPlayerTransform;
    }

    public class GroundPatrolState : EnemyState
    {
        private GroundPatrolSettings settings;
        private bool hasFoundPlayer;

        public GroundPatrolState(GroundPatrolSettings _settings)
        {
            stateName = States.GroundPatrol;
            settings = _settings;
            settings.detectPlayer.onPlayerFound += PlayerFound;
        }



        public override States CheckExitConditions()
        {
            if (hasFoundPlayer)
            {
                return States.GroundChase;
            }
            return States.UNCHANGED;
        }

        public override void Enter()
        {

        }

        public override void Exit()
        {
            settings.detectPlayer.onPlayerFound -= PlayerFound;
        }

        public override void UpdateState()
        {
            settings.detectPlayer.SearchForPlayer();
        }

        public override void FixedUpdateState()
        {
            Move(settings.ownerTransform, settings.ownerRb, settings.speed);
        }

        private void Move(Transform transform, Rigidbody2D rb, float speed)
        {
            if (CheckPath(settings.type))
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
            rb.velocity = new Vector2(transform.localScale.x * speed * Time.deltaTime, rb.velocity.y);
        }

        private bool CheckPath(GroundPatrolSettings.PatrolType patroltype)
        {
            switch (patroltype)
            {
                case GroundPatrolSettings.PatrolType.Walls:
                    return Physics2D.OverlapCircle(settings.WallCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
                case GroundPatrolSettings.PatrolType.Cliffs:
                    return !Physics2D.OverlapCircle(settings.CliffCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
                case GroundPatrolSettings.PatrolType.Both:
                    return !Physics2D.OverlapCircle(settings.CliffCheckTransform.position, settings.CheckRadius, settings.CheckLayer) ||
                            Physics2D.OverlapCircle(settings.WallCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
                default:
                    return false;
            }
        }

        private void PlayerFound(Transform playerTransform)
        {
            hasFoundPlayer = true;
            settings.TargetPlayerTransform = playerTransform;
        }
    }
}
