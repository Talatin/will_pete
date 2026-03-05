using System;
using Assets.Scripts.Player;
using UnityEngine;
using World;

namespace Items
{
    [RequireComponent(typeof(SpriteRenderer), typeof(BoxCollider2D),typeof(Rigidbody2D))]
    public abstract class Item : MonoBehaviour, IKnockable
    {
        [SerializeField] private Vector2 carryOffset;
        
        protected PlayerController Player;
        protected SpriteRenderer SpriteRend;
        private Rigidbody2D rb;
        private Collider2D col;
        
        public Vector2 CarryOffset => carryOffset;

        protected virtual void OnRemoveFromPlayer()
        {
            Player.PlayerState.OnFacingDirectionChanged -= ChangeOffsetPosition;
        }
        
        protected virtual void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            SpriteRend = GetComponent<SpriteRenderer>();
        }

        public virtual Item Pickup(PlayerController player)
        {
            Player = player;
            Player.PlayerState.OnFacingDirectionChanged += ChangeOffsetPosition;
            ChangeState(false);
            return this;
        }

        private void ChangeOffsetPosition()
        {
            transform.localPosition = new Vector3(Player.PlayerState.IsFacingRight ? carryOffset.x : -carryOffset.x, carryOffset.y, 0);
        }

        public virtual void Drop()
        {
            Throw(Vector2.up, 1);
        }

        public virtual void Throw(Vector2 direction, float power)
        {
            direction.Normalize();
            transform.parent = null;
            ChangeState(true);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(direction * power, ForceMode2D.Impulse);
        }

        protected virtual void ChangeState(bool isOnGround)
        {
            rb.simulated = isOnGround;
            col.enabled = isOnGround;

            if (isOnGround)
            {
                OnRemoveFromPlayer();
                Player = null;
            }
        }
        
        public void Knockback(Vector2 point, Vector2 origin, float knockbackForce)
        {
            if (!rb)
            {
                rb = transform.GetComponent<Rigidbody2D>();
            }

            Vector2 direction = point - origin;
            rb.AddForceAtPosition(direction.normalized * knockbackForce,point, ForceMode2D.Impulse);
        }
    }
}