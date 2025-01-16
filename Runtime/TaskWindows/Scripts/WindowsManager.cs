using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime
{
    public class WindowsManager : MonoBehaviour
    {
        [SerializeField] private WindowConfigurationsCollection _windowConfigsCollection;

        [SerializeField] private GameObject _explorerRoot;
        [SerializeField] private Button _explorerButton;
        [SerializeField] private Explorer _explorer;

        [SerializeField] private RectTransform _activeTaskIconsRoot;
        [SerializeField] private RectTransform _activeTaskWindowsRoot;

        private List<Window> _openWindows = new List<Window>();
        private List<WindowIcon> _openWindowIcons = new List<WindowIcon>();

        private void Start()
        {
            foreach (Transform child in _activeTaskIconsRoot)
            {
                Destroy(child.gameObject);
            }

            _explorerButton.onClick.AddListener(ToggleExplorer);
            _explorer.Initialise(this, _windowConfigsCollection);
            _explorerRoot.SetActive(false);
        }

        public void ToggleExplorer()
        {
            _explorerRoot.SetActive(_explorerRoot.activeSelf == false);
        }

        public void HideExplorer()
        {
            _explorerRoot.SetActive(false);
        }

        public Window OpenWindow(WindowConfiguration windowConfiguration)
        {
            Window window = Instantiate(windowConfiguration.WindowPrefab, _activeTaskWindowsRoot).GetComponent<Window>();
            _openWindows.Add(window);

            WindowIcon windowIcon = Instantiate(windowConfiguration.IconPrefab, _activeTaskIconsRoot).GetComponent<WindowIcon>();
            _openWindowIcons.Add(windowIcon);

            window.Initialise(this, windowConfiguration, windowIcon);

            window.OnWindowClosed += HandleWindowClosed;

            HideExplorer();

            return window;
        }

        private void HandleWindowClosed(Window closedWindow)
        {
            _openWindows.Remove(closedWindow);
            _openWindowIcons.Remove(closedWindow.WindowIcon);
        }
    }
}