using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public class QC_PolyCircleCollider : QC_Collider
    {
        public Vector2 _centre;
        public float _radius;

        public override Bounds GetWorldBounds()
        {
            return new Bounds(transform.position + (Vector3)_centre, Vector2.one * (_radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y)));
        }
        
        public override bool CheckForCollision(QC_Collider other)
        {
            throw new NotImplementedException();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + (Vector3)_centre, Vector2.one * (_radius * Mathf.Max(transform.lossyScale.x, transform.lossyScale.y)));
        }
    }
}
