using System;
using UnityEngine;

namespace Assets.Scripts.Enemies
{


    public class GoblinController : MonoBehaviour
    {
        public GroundPatrolSettings groundPatrolSettings;
        public GroundChaseSettings groundChaseSettings;
        public GroundAttackSettings groundAttackSettings;
        private EnemyState currentState;
        private GoblinHealth goblinHealth;
        private enum Strategies { patrol, chase }
        private void Awake()
        {
            ChangeState(EnemyState.States.GroundPatrol);

            goblinHealth = GetComponent<GoblinHealth>();
            goblinHealth.died += OnDeath;
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.UpdateState();
                EnemyState.States result = currentState.CheckExitConditions();
                if (result != currentState.stateName && result != EnemyState.States.UNCHANGED)
                {
                    ChangeState(result);
                }
            }
        }

        private void FixedUpdate()
        {
            if (currentState != null)
            {
                currentState.FixedUpdateState();
            }
        }


        private void OnDeath()
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            //setState to dead
            Destroy(gameObject, 0.25f);
        }

        private void ChangeState(EnemyState.States state)
        {
            if (currentState != null)
            {
                Debug.Log($"transitioned from [{currentState}]");
                currentState.Exit();
            }
            switch (state)
            {
                case EnemyState.States.UNCHANGED:
                    return;
                case EnemyState.States.GroundPatrol:
                    currentState = new GroundPatrolState(groundPatrolSettings);
                    break;
                case EnemyState.States.GroundChase:
                    groundChaseSettings.target = groundPatrolSettings.TargetPlayerTransform;
                    currentState = new GroundChaseState(groundChaseSettings);
                    break;
                case EnemyState.States.GroundAttack:
                    groundAttackSettings.targetTransform = groundChaseSettings.target;
                    currentState = new GroundAttackState(groundAttackSettings);
                    //currentState = new GroundPatrolState(groundPatrolSettings);
                    break;
                default:
                    break;
            }
            
            Debug.Log($"transitioned to [{currentState}]");
            currentState.Enter();
        }

        private void OnDrawGizmos()
        {
            if (groundPatrolSettings.isDrawingGizmos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundPatrolSettings.WallCheckTransform.position, groundPatrolSettings.CheckRadius);
                Gizmos.DrawWireSphere(groundPatrolSettings.CliffCheckTransform.position, groundPatrolSettings.CheckRadius);
            }

            if(groundChaseSettings.isDrawingGizmos)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(groundChaseSettings.GroundCheckTransform.position, groundChaseSettings.CheckRadius);
                Gizmos.DrawWireSphere(groundChaseSettings.CliffCheckTransform.position, groundChaseSettings.CheckRadius);
                Gizmos.DrawWireSphere(groundChaseSettings.WallCheckTransform.position, groundChaseSettings.WallCheckRadius);
            }
        }
    }
}