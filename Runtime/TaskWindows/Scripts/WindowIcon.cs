using System;
using UnityEngine;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime
{
    public class WindowIcon : MonoBehaviour
    {
        [SerializeField] private Button _maximiseWindowBtn;

        private void Awake()
        {
            _maximiseWindowBtn.onClick.AddListener(MaximiseWindow);
        }

        private void MaximiseWindow() { }
    }
}