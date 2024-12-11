using System;
using UnityEngine;


namespace Assets.Scripts.Enemies
{
    [Serializable]
    public class GroundAttackSettings
    {
        public float AttackDuration;
        public float LungePower;
        public float TimeTillAttack;
        public Rigidbody2D ownerRb;
        public Transform ownerTransform;
        public DetectPlayer detectPlayer;
    }

    internal class GroundAttackState : EnemyState
    {
        private GroundAttackSettings settings;
        private float timeAfterStrike;
        private float currentTimeTillStrike;
        private bool hasAttacked = false;

        public GroundAttackState(GroundAttackSettings settings)
        {
            this.settings = settings;
            timeAfterStrike += settings.TimeTillAttack;
            hasAttacked = false;
        }

        public override States CheckExitConditions()
        {
            timeAfterStrike += Time.deltaTime;
            if (timeAfterStrike > settings.AttackDuration)
            {
                return States.GroundChase;
            }
            if (settings.detectPlayer.PlayerTransform == null)
            {
                return States.GroundPatrol;
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
            settings.ownerRb.velocity = Vector2.Lerp(settings.ownerRb.velocity, Vector2.zero, Time.deltaTime * 2);
        }

        public override void UpdateState()
        {
            currentTimeTillStrike += Time.deltaTime;
            if (currentTimeTillStrike >= settings.TimeTillAttack && !hasAttacked)
            {
                settings.ownerRb.AddForce((settings.detectPlayer.PlayerTransform.position - settings.ownerTransform.position).normalized * settings.LungePower, ForceMode2D.Impulse);
                hasAttacked = true;
            }
        }
    }
}
