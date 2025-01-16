using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace GoodHub.Core.Runtime
{
    public class WindowConfiguration : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private GameObject _windowPrefab;
        [SerializeField] private GameObject _iconPrefab;

        public string Name => _name;
        public GameObject WindowPrefab => _windowPrefab;
        public GameObject IconPrefab => _iconPrefab;
    }
}