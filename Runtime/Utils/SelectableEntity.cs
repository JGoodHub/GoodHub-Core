using System;
using System.Collections;
using System.Collections.Generic;
using GoodHub.Core.Runtime.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GoodHub.Core.Runtime
{
    public class SelectableEntity : MonoBehaviour
    {
        [Header("Entity")]
        [SerializeField] private string _name;
        [SerializeField] private int _guid;
        [Space]
        [SerializeField] private GameObject _selectionRing;
        [SerializeField] private float _selectionRadius;
        [SerializeField] private int _selectionPriority; // Higher == More Important

        public float SelectionRadius => _selectionRadius;

        public GameObject SelectionRing => _selectionRing;

        protected virtual void Start()
        {
            SelectionController.Singleton.RegisterEntity(this);
            SelectionStatusChanged(false);

            _guid = Random.Range(0, 1000000);
        }

        private void OnDestroy()
        {
            if (SelectionController.Singleton)
                SelectionController.Singleton.UnregisterEntity(this);
        }

        public virtual void SelectionStatusChanged(bool isSelected)
        {
            if (_selectionRing != null)
            {
                _selectionRing.SetActive(isSelected);
            }
        }

        protected virtual void OnDrawGizmos()
        {
            GizmosUtil.DrawWireRing(transform.position, _selectionRadius, 128, Color.cyan);
        }
    }
}