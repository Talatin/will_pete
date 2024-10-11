using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class GoblinController : MonoBehaviour
    {
        private DetectPlayer detectPlayer;
        private IEnemyMoveStrategy currentStrategy;
        private GoblinHealth goblinHealth;
        private enum Strategies { patrol, chase }
        private bool isDead = false;
        private bool isStunned = false;
        private PatrolStrategy patrolStrategy;
        private ChaseStrategy chaseStrategy;
        private float playerFoundTimeStamp;
        // Start is called before the first frame update

        private void Awake()
        {
            patrolStrategy = GetComponent<PatrolStrategy>();
            chaseStrategy = GetComponent<ChaseStrategy>();
            currentStrategy = patrolStrategy;

            goblinHealth = GetComponent<GoblinHealth>();
            goblinHealth.died += OnDeath;
            detectPlayer = GetComponent<DetectPlayer>();
            detectPlayer.onPlayerFound += OnPlayerFound;
        }

        private void OnDeath()
        {
            isDead = true;
            GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(gameObject, 0.25f);
        }

        private void OnPlayerFound(Vector3 pos)
        {
            Debug.Log("Player found");
            chaseStrategy.LastPlayerPosition = pos;
            currentStrategy = chaseStrategy;
            playerFoundTimeStamp = Time.time;

        }

        private void FixedUpdate()
        {
            if (!isDead && !isStunned)
            {
                currentStrategy.Move();

            }
            if (!isDead)
            {
                detectPlayer.SearchForPlayer();
            }

            //if(detectPlayer.SearchForPlayer())
            //{
            //    currentStrategy = chaseStrategy;
            //    chaseStrategy.LastPlayerPosition = 
            //}
            //else
            //{
            //    currentStrategy = patrolStrategy;
            //}
        }

        private IEnumerator CollisionMoveStop()
        {
            isStunned = true;
            yield return new WaitForSeconds(1);
            isStunned = false;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                player.TakeDamage(PlayerHealth.DamageType.Knockback, transform);
                StartCoroutine(CollisionMoveStop());
            }
        }


    }

}