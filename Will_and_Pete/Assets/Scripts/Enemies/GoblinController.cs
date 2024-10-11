using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class GoblinController : MonoBehaviour
    {
        private DetectPlayer detectPlayer;
        private IEnemyMoveStrategy currentStrategy;
        private GoblinHealth goblinHealth;
        private enum Strategies { patrol, chase }
        private bool dead = false;
        private bool stopMovement = false;
        private PatrolStrategy patrolStrategy;
        private ChaseStrategy chaseStrategy;
        [SerializeField] private float timeToIdle;
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
            dead = true;
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

        private void Update()
        {
            //if (Time.time - playerFoundTimeStamp < timeToIdle)
            //{
            //    currentStrategy = patrolStrategy;
            //}
        }

        private void FixedUpdate()
        {
            if (!dead && !stopMovement)
            {
                currentStrategy.Move();

            }
            if (!dead)
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                player.TakeDamage(PlayerHealth.DamageType.Knockback, transform);
                StartCoroutine(CollisionMoveStop());
            }
        }

        private IEnumerator CollisionMoveStop()
        {
            stopMovement = true;
            yield return new WaitForSeconds(1);
            stopMovement = false;
        }

    }

}