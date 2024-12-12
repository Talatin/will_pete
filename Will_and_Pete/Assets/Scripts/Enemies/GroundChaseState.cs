using System;
using System.Buffers;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Enemies
{

    [Serializable]
    public class GroundChaseSettings
    {
        public bool isDrawingGizmos;
        [Header("Components")]
        public Transform ownerTransform;
        public Rigidbody2D ownerRb;
        public DetectPlayer detectPlayer;
        [Header("State Variables")]
        public float speed;
        public float jumpPower;
        public float attackRange;
        public enum ChaseType { flat, jump }
        public ChaseType type;
        [Header("Collision Checks")]
        public Transform CliffCheckTransform;
        public Transform GroundCheckTransform;
        public float GroundCliffCheckRadius;
        public Transform WallCheckTransform;
        public float WallCheckRadius;
        public LayerMask CheckLayer;
    }

    public class GroundChaseState : EnemyState
    {
        private readonly GroundChaseSettings settings;
        private bool isInAttackRange;
        private bool isGrounded;
        public GroundChaseState(GroundChaseSettings _settings)
        {
            settings = _settings;
        }

        public override States CheckExitConditions()
        {
            if (settings.detectPlayer.PlayerTransform == null)
            {
                return States.GroundPatrol;
            }
            if (isInAttackRange)
            {
                return States.GroundAttack;
            }
            return States.UNCHANGED;
        }

        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void FixedUpdateState()
        {
            isGrounded = Physics2D.OverlapCircle(settings.GroundCheckTransform.position, settings.GroundCliffCheckRadius, settings.CheckLayer);
            Move();
            GravityManipulation();
            
        }

        public override void UpdateState()
        {
        }


        public void Move()
        {
            if (isGrounded)
            {
                if (!CheckAttackRange())
                {
                    settings.ownerRb.velocity = new Vector2(settings.ownerTransform.localScale.x * settings.speed * Time.deltaTime, settings.ownerRb.velocity.y);
                    LookAtPlayer();
                }
                else
                {
                    settings.ownerRb.velocity = new Vector2(0, settings.ownerRb.velocity.y);
                    //change to attack state
                    isInAttackRange = true;
                }
            }
            else
            {
                settings.ownerRb.velocity = Vector2.Lerp(settings.ownerRb.velocity, new Vector2(settings.ownerTransform.localScale.x * settings.speed * Time.deltaTime, settings.ownerRb.velocity.y), Time.fixedDeltaTime * 1);
            }

            bool isAtCliff = !Physics2D.OverlapCircle(settings.CliffCheckTransform.position, settings.GroundCliffCheckRadius, settings.CheckLayer);
            bool isTargetAcrossCliff = false;
            if (settings.detectPlayer.PlayerTransform != null)
            {
                isTargetAcrossCliff = settings.detectPlayer.PlayerTransform.position.y >= (settings.ownerTransform.position.y - settings.ownerTransform.localScale.y / 2);
            }
            bool isBlockedByMap = Physics2D.OverlapCircle(settings.WallCheckTransform.position, settings.WallCheckRadius, settings.CheckLayer);

            if (isBlockedByMap || (isAtCliff && isTargetAcrossCliff))
            {
                Jump();
            }
        }

        private void LookAtPlayer()
        {
            if (settings.detectPlayer.PlayerTransform.position.x < settings.ownerTransform.position.x)
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
            float distance = Mathf.Abs(settings.detectPlayer.PlayerTransform.position.x - settings.ownerTransform.position.x);
            return distance < settings.attackRange;
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
            if (isGrounded)
            {
                isGrounded = false;
                settings.ownerRb.velocity = new Vector2(settings.ownerRb.velocity.x, 0);
                settings.ownerRb.AddForce(Vector2.up * settings.jumpPower, ForceMode2D.Impulse);
            }
        }
    }
}