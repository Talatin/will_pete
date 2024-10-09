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
        private enum Strategies {patrol,chase }
        private bool dead = false;
        private bool stopMovement = false;
        // Start is called before the first frame update

        private void Awake()
        {
            currentStrategy = GetComponent<IEnemyMoveStrategy>();
            goblinHealth = GetComponent<GoblinHealth>();
            goblinHealth.died += OnDeath;
            detectPlayer = GetComponent<DetectPlayer>();
        }

        

        private void OnDeath()
        {
           dead = true;
           GetComponent<SpriteRenderer>().color = Color.red;
            Destroy(gameObject,0.25f);
        }

        private void Changestrategy(Strategies strat)
        {

        }

        void FixedUpdate()
        {
            if (!dead && !stopMovement)
            {
                currentStrategy.Move(); 
            }
            if(detectPlayer.SearchForPlayer())
            {
                Changestrategy(Strategies.chase);
            }
            else
            {
                Changestrategy(Strategies.patrol);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.TryGetComponent<PlayerHealth>(out PlayerHealth player))
            {
                player.TakeDamage(PlayerHealth.DamageType.Knockback,transform);
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