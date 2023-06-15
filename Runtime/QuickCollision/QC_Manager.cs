using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime.QuickCollision
{
    public static class QC_Manager
    {
        private static HashSet<QC_Collider> _colliders;

        static QC_Manager()
        {
            _colliders = new HashSet<QC_Collider>();
        }

        public static void RegisterCollider(QC_Collider collider)
        {
            if (_colliders.Contains(collider))
                return;

            _colliders.Add(collider);
        }

        public static void UnregisterCollider(QC_Collider collider)
        {
            if (_colliders.Contains(collider) == false)
                return;

            _colliders.Remove(collider);
        }

        public static void CheckForCollisions()
        {
            HashSet<IntersectionPair> broadPhaseIntersections = GetBroadPhaseIntersections();

            HashSet<IntersectionPair> narrowPhaseIntersections = GetNarrowPhaseIntersections(broadPhaseIntersections);

            foreach (IntersectionPair intersectionPair in narrowPhaseIntersections)
            {
                intersectionPair.ColliderA.OnCollisionStay?.Invoke(intersectionPair.ColliderB);
                intersectionPair.ColliderB.OnCollisionStay?.Invoke(intersectionPair.ColliderA);
            }
        }

        private static HashSet<IntersectionPair> GetBroadPhaseIntersections()
        {
            HashSet<IntersectionPair> broadPhaseIntersections = new HashSet<IntersectionPair>();

            foreach (QC_Collider colliderA in _colliders)
            {
                foreach (QC_Collider colliderB in _colliders)
                {
                    if (colliderA == colliderB)
                        continue;

                    if (colliderA.GetBounds().Intersects(colliderB.GetBounds()) == false)
                        continue;

                    IntersectionPair intersectionPair = new IntersectionPair(colliderA, colliderB);

                    if (broadPhaseIntersections.Contains(intersectionPair))
                        continue;

                    broadPhaseIntersections.Add(intersectionPair);
                }
            }

            return broadPhaseIntersections;
        }
        
        private static HashSet<IntersectionPair> GetNarrowPhaseIntersections(HashSet<IntersectionPair> broadPhaseIntersections)
        {
            HashSet<IntersectionPair> narrowPhaseIntersections = new HashSet<IntersectionPair>();

            foreach (IntersectionPair intersectionPair in broadPhaseIntersections)
            {
                if (intersectionPair.ColliderA.IsOverlapping(intersectionPair.ColliderB))
                {
                    narrowPhaseIntersections.Add(intersectionPair);
                }
            }

            return narrowPhaseIntersections;
        }
    }

    public readonly struct IntersectionPair
    {
        private readonly QC_Collider _colliderA;
        private readonly QC_Collider _colliderB;

        public QC_Collider ColliderA => _colliderA;
        public QC_Collider ColliderB => _colliderB;

        public IntersectionPair(QC_Collider colliderA, QC_Collider colliderB)
        {
            _colliderA = colliderA;
            _colliderB = colliderB;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            IntersectionPair otherPair = (IntersectionPair)obj;

            if (otherPair.ColliderA == _colliderA && otherPair.ColliderB == _colliderB)
                return true;

            if (otherPair.ColliderA == _colliderB && otherPair.ColliderB == _colliderA)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hashCodeA = _colliderA.GetHashCode();
            int hashCodeB = _colliderB.GetHashCode();

            if (hashCodeA < hashCodeB)
                return HashCode.Combine(_colliderA, _colliderB);

            return HashCode.Combine(_colliderB, _colliderA);
        }
    }
}
