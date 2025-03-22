using System;
using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.World;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Player
{
    public class PlayerThrowing : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private LayerMask throwableLayer;

        private Rigidbody2D heldObject;
        private PlayerSettings pSettings;
        private PlayerState pState;
        private Rigidbody2D rb;
        private bool recentlyThrown;
        private IEnumerator coroutine;
        private Coroutine throwCoroutine;

        public void Initialize(PlayerState state, PlayerSettings settings)
        {
            pState = state;
            pSettings = settings;
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (heldObject)
            {
                heldObject.position = spawnPosition.position;
            }

            if (!heldObject && !recentlyThrown)
            {
                Collider2D check = Physics2D.OverlapCircle(transform.position, 1, throwableLayer);
                if (check)
                {
                    CatchItem(check.gameObject);
                }
            }
        }

        public void Throw()
        {
            if (!heldObject)
            {
                return;
            }

            Vector2 throwDirection = rb.velocity == Vector2.zero
                ? pState.IsFacingRight ? new Vector2(1, 1) : new Vector2(-1, 1)
                : rb.velocity;
            throwDirection.Normalize();

            IThrowable throwable = heldObject.GetComponent<IThrowable>();
            heldObject = null;
            throwable.ToggleRigidbody(true);
            throwable.ToggleCollider(true);
            throwable.Throw(rb.velocity.magnitude / 2 + 10, throwDirection);


            if (throwCoroutine == null)
            {
                coroutine = ThrowCooldown();
                throwCoroutine = StartCoroutine(coroutine);
            }
        }

        private void CatchItem(GameObject item)
        {
            if (item.TryGetComponent(out IThrowable throwable))
            {
                throwable.ToggleCollider(false);
                throwable.ToggleRigidbody(false);
                heldObject = throwable.GetRigidbody();
                throwable.SetParent(transform);
            }
        }

        private IEnumerator ThrowCooldown()
        {
            recentlyThrown = true;
            yield return new WaitForSeconds(0.4f);
            recentlyThrown = false;
            throwCoroutine = null;
        }
    }
}