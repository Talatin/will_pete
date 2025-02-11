using System;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class PlayerCarry : MonoBehaviour
    {
        [SerializeField] private Joint2D joint2D;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
            {
                joint2D.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
                joint2D.enabled = true;
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject == joint2D.attachedRigidbody.gameObject)
            {
                joint2D.connectedBody = null;
                joint2D.enabled = false;
            }
        }
    }
}