using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GoodHub.Core.Runtime
{

    public class SelectableEntity : MonoBehaviour
    {

        [Header("Entity")]
        public new string name;
        public int id;
        public Type type;
        [Space]
        [SerializeField] private GameObject selectionRing;
        [SerializeField] private float selectionRadius;

        public float SelectionRadius => selectionRadius;

        protected virtual void Start()
        {
            SelectionController.Instance.RegisterEntity(this);
            SetSelected(false);

            id = Random.Range(0, 1000000);
        }

        private void OnDestroy()
        {
            SelectionController.Instance.UnregisterEntity(this);
        }

        public void SetSelected(bool state)
        {
            if (selectionRing != null)
                selectionRing.SetActive(state);
        }

    }

}