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
        // Start is called before the first frame update

        private void Awake()
        {
            currentStrategy = GetComponent<IEnemyMoveStrategy>();
            goblinHealth = GetComponent<GoblinHealth>();
            goblinHealth.died += OnDeath;
        }

        private void OnDeath()
        {
           dead = true;
           GetComponent<SpriteRenderer>().color = Color.red;
        }

        private void Changestrategy(Strategies strat)
        {
        }

        void FixedUpdate()
        {
            if (!dead)
            {
                currentStrategy.Move(); 
            }
        }
    }

}