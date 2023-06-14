using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public abstract class QC_Collider : MonoBehaviour
    {
        public delegate void OnCollision(QC_Collider other);

        public OnCollision OnCollisionEnter;
        public OnCollision OnCollisionStay;
        public OnCollision OnCollisionExit;

        protected virtual void Start()
        {
            QC_Manager.RegisterCollider(this);
        }

        protected virtual void OnDestroy()
        {
            QC_Manager.UnregisterCollider(this);
        }

        public abstract Bounds GetWorldBounds();

        public abstract bool CheckForCollision(QC_Collider other);
    }
}