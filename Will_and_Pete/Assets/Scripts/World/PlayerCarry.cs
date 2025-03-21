using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class PlayerCarry : MonoBehaviour
    {
        private List<Rigidbody2D> jointRigidbody2Ds = new List<Rigidbody2D>();
        [SerializeField] private Rigidbody2D rb;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
            {
                //Joint2D joint = CreateNewJoint();
                //joint.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();

                ConnectNewBody(other.gameObject.GetComponent<Rigidbody2D>());
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D otherBody))
            {
                if (jointRigidbody2Ds.Contains(otherBody))
                {
                    DisconnectBody(collision.gameObject.GetComponent<Rigidbody2D>());
                }
            }
        }
        

        private void FixedUpdate()
        {
            return;
            MoveConnectedBodies();
        }

        private Joint2D CreateNewJoint()
        {
            SpringJoint2D spring = gameObject.AddComponent<SpringJoint2D>();
            spring.enabled = true;
            spring.enableCollision = true;
            spring.autoConfigureConnectedAnchor = true;
            spring.dampingRatio = 1;
            spring.frequency = 0;
            spring.breakForce = 1;
            spring.breakAction = JointBreakAction2D.Destroy;
            spring.autoConfigureDistance = true;
            spring.distance = 0.005f;
            return spring;
        }

        private void MoveConnectedBodies()
        {
            foreach (Rigidbody2D jointBody in jointRigidbody2Ds)
            {
                jointBody.position = Vector2.Lerp(jointBody.position,transform.position + new Vector3(0, 0.5f, 0),Time.deltaTime * 5);
            }
        }

        private void ConnectNewBody(Rigidbody2D body)
        {
            if (jointRigidbody2Ds.Contains(body))
            {
                return;
            }
            jointRigidbody2Ds.Add(body);
        }

        private void DisconnectBody(Rigidbody2D body)
        {
            jointRigidbody2Ds.Remove(body);
        }
    }
}