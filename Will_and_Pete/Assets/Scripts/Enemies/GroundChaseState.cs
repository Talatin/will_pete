using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{

    [Serializable]
    public class GroundChaseSettings
    {
        public bool isDrawingGizmos;
        [Space]
        public Transform ownerTransform;
        public Rigidbody2D ownerRb;
        public DetectPlayer detectPlayer;
        public float speed;
        public float jumpPower;
        public float attackRange;
        public enum ChaseType { flat, jump }
        public ChaseType type;
        public Transform CliffCheckTransform;
        public Transform GroundCheckTransform;
        public float CheckRadius;
        public Transform WallCheckTransform;
        public float WallCheckRadius;
        public LayerMask CheckLayer;
        public bool isGrounded;
        public Transform target;

    }

    public class GroundChaseState : EnemyState
    {
        private readonly GroundChaseSettings settings;
        public GroundChaseState(GroundChaseSettings _settings)
        {
            settings = _settings;
        }

        public override States CheckExitConditions()
        {
            return States.UNCHANGED;
        }

        public override void Enter()
        {
            settings.detectPlayer.onPlayerFound += CurrentTarget;
        }

        private void CurrentTarget(Transform playerTransform)
        {
            settings.target = playerTransform;
        }

        public override void Exit()
        {
            settings.detectPlayer.onPlayerFound -= CurrentTarget;
        }

        public override void FixedUpdateState()
        {
            settings.isGrounded = Physics2D.OverlapCircle(settings.GroundCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
            Move();
            GravityManipulation();
            settings.detectPlayer.SearchForPlayer();
        }

        public override void UpdateState()
        {
        }


        public void Move()
        {
            if (settings.isGrounded)
            {
                if (!CheckAttackRange())
                {
                    settings.ownerRb.velocity = new Vector2(settings.ownerTransform.localScale.x * settings.speed * Time.deltaTime, settings.ownerRb.velocity.y);
                    LookAtPlayer();
                }
                else
                {
                    //change to attack state
                }
            }
            else
            {
                settings.ownerRb.velocity = Vector2.Lerp(settings.ownerRb.velocity, new Vector2(settings.ownerTransform.localScale.x * settings.speed * Time.deltaTime, settings.ownerRb.velocity.y), Time.fixedDeltaTime * 1);
            }

            bool isAtCliff = !Physics2D.OverlapCircle(settings.CliffCheckTransform.position, settings.CheckRadius, settings.CheckLayer);
            bool isTargetAcrossCliff = settings.target.position.y >= (settings.ownerTransform.position.y - settings.ownerTransform.localScale.y / 2);
            bool isBlockedByMap = Physics2D.OverlapCircle(settings.WallCheckTransform.position, settings.WallCheckRadius, settings.CheckLayer);

            if (isBlockedByMap || (isAtCliff && isTargetAcrossCliff))
            {
                Jump();
            }
        }

        private void LookAtPlayer()
        {
            if (settings.target.position.x < settings.ownerTransform.position.x)
            {
                settings.ownerTransform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                settings.ownerTransform.localScale = Vector3.one;
            }
        }

        private bool CheckAttackRange()
        {
            return (settings.target.position - settings.ownerTransform.position).magnitude < settings.attackRange;
        }

        private void GravityManipulation()
        {
            // Set Charactergravity according to current y velocity and jump input
            if (settings.ownerRb.velocity.y < -0.1f)
            {
                settings.ownerRb.gravityScale = 2;
            }
            else if (settings.ownerRb.velocity.y >= 0)
            {
                settings.ownerRb.gravityScale = 1;
            }
        }

        private void Jump()
        {
            if (settings.isGrounded)
            {
                settings.isGrounded = false;
                settings.ownerRb.velocity = new Vector2(settings.ownerRb.velocity.x, 0);
                settings.ownerRb.AddForce(Vector2.up * settings.jumpPower, ForceMode2D.Impulse);
            }
        }
    }
}