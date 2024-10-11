﻿using UnityEngine;

namespace Assets.Scripts.Enemies
{
    public class GoblinHealth : MonoBehaviour, IDamageable
    {
        Rigidbody2D rb;
        public delegate void Died();
        public event Died died;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void TakeDamage()
        {
            rb.velocity = Vector3.zero;
            rb.drag = 4f;
            rb.AddForce(new Vector2(-transform.localScale.x * 3, 5), ForceMode2D.Impulse);
            died?.Invoke();
        }
    }
}