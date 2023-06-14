using System;
using UnityEngine;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public class ColliderTester : MonoBehaviour
    {

        public SpriteRenderer spriteRenderer;
        public QC_Collider qcCollider;

        private void Start()
        {
            qcCollider.OnCollisionEnter += CollisionEnter;
            qcCollider.OnCollisionStay += CollisionStay;
            qcCollider.OnCollisionExit += CollisionExit;
        }

        private void Update()
        {
            spriteRenderer.color = Color.cyan;
        }

        private void CollisionEnter(QC_Collider other)
        {
            Debug.Log($"{gameObject.name} collided with {other.gameObject.name}");
        }

        private void CollisionStay(QC_Collider other)
        {
            spriteRenderer.color = Color.red;
        }

        private void CollisionExit(QC_Collider other)
        {
            spriteRenderer.color = Color.green;
        }


    }
}
