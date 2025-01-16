using UnityEngine;
using UnityEngine.UI;

namespace GoodHub.Core.Runtime
{
    public class OpenWindowMenuItem : MonoBehaviour
    {
        [SerializeField] private Button _openTaskWindowButton;
        [SerializeField] private RectTransform _iconRoot;
        [SerializeField] private Text _taskName;

        private WindowsManager _windowsManager;
        private WindowConfiguration _windowConfig;

        private void Awake()
        {
            _openTaskWindowButton.onClick.AddListener(OpenTaskWindow);
        }

        public void Initialise(WindowsManager windowsManager,WindowConfiguration windowConfig)
        {
            _windowsManager = windowsManager;
            _windowConfig = windowConfig;
        }

        private void OpenTaskWindow()
        {
            _windowsManager.OpenWindow(_windowConfig);
        }
    }
}