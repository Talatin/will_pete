using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


namespace Assets.Scripts.Enemies
{
    [Serializable]
    public class GroundAttackSettings
    {
        [Header("Components")] public Transform ownerTransform;
        public Rigidbody2D ownerRb;
        public DetectPlayer detectPlayer;
        public Transform SpearHolder;
        public BoxCollider2D AttackHitBox;
        [Header("State Variables")] public float AttackCooldown;
        public float LungePower;
        public float ChargeDuration;
    }

    internal class GroundAttackState : EnemyState
    {
        private GroundAttackSettings settings;
        private float currentTimeTillStrike;
        private bool hasAttacked = false;

        public GroundAttackState(GroundAttackSettings settings)
        {
            this.settings = settings;
            hasAttacked = false;
        }

        public override States CheckExitConditions()
        {
            currentTimeTillStrike += Time.deltaTime;
            if (currentTimeTillStrike > settings.AttackCooldown + settings.ChargeDuration)
            {
                if (!settings.detectPlayer.PlayerTransform)
                {
                    return States.GroundPatrol;
                }

                return States.GroundChase;
            }

            return States.UNCHANGED;
        }

        public override void Enter()
        {
            settings.ownerRb.velocity = new Vector2(0, settings.ownerRb.velocity.y);
            RotateToTarget(settings.ownerTransform.right);
        }

        public override void Exit()
        {
            RotateToTarget(Vector2.up);
            settings.ownerRb.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            settings.AttackHitBox.enabled = false;
        }

        public override void FixedUpdateState()
        {
            settings.ownerRb.velocity = Vector2.Lerp(settings.ownerRb.velocity, Vector2.zero, Time.deltaTime * 1.5f);
        }

        public override void UpdateState()
        {
            currentTimeTillStrike += Time.deltaTime;
            if (currentTimeTillStrike >= settings.ChargeDuration && !hasAttacked)
            {
                Vector2 dir = settings.detectPlayer.PlayerTransform.position - settings.ownerTransform.position;
                dir.y = 0;
                dir.Normalize();
                settings.ownerRb.AddForce(dir * settings.LungePower, ForceMode2D.Impulse);
                settings.AttackHitBox.enabled = true;
                hasAttacked = true;
            }
        }

        public void RotateToTarget(Vector2 direction)
        {
            settings.SpearHolder.transform.right = direction;
        }
    }
}