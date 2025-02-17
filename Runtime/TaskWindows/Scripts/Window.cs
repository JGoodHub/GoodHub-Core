using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private Button _minimiseButton;
        [SerializeField] private Button _closeButton;

        protected bool _isVisible;
        protected WindowsManager _windowsManager;
        protected WindowConfiguration _windowConfig;
        protected WindowIcon _windowIcon;

        public event Action<Window> OnWindowMinimised = null;
        public event Action<Window> OnWindowClosed = null;

        public WindowIcon WindowIcon => _windowIcon;

        private void Awake()
        {
            if (_minimiseButton != null)
            {
                _minimiseButton.onClick.AddListener(MinimiseWindow);
            }

            if (_closeButton != null)
            {
                _closeButton.onClick.AddListener(CloseWindow);
            }
        }

        public void Initialise(WindowsManager windowsManager, WindowConfiguration windowConfig, WindowIcon windowIcon)
        {
            _windowsManager = windowsManager;
            _windowConfig = windowConfig;
            _windowIcon = windowIcon;
            
            OnInitialised();
        }

        protected virtual void OnInitialised() { }

        public void MinimiseWindow()
        {
            OnWindowMinimised?.Invoke(this);
        }

        public void CloseWindow()
        {
            Destroy(_windowIcon.gameObject);
            Destroy(gameObject);

            OnWindowClosed?.Invoke(this);
        }
    }
}