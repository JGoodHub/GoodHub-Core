using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime
{
    public class Explorer : MonoBehaviour
    {
        [SerializeField] private GameObject _menuItemPrefab;
        [SerializeField] private RectTransform _menuItemsRoot;

        public void Initialise(WindowsManager windowsManager, WindowConfigurationsCollection windowConfigsCollection)
        {
            foreach (Transform child in _menuItemsRoot)
            {
                Destroy(child.gameObject);
            }

            foreach (WindowConfiguration windowConfig in windowConfigsCollection.WindowConfigurations)
            {
                OpenWindowMenuItem windowMenuItem = Instantiate(_menuItemPrefab, _menuItemsRoot).GetComponent<OpenWindowMenuItem>();
                windowMenuItem.Initialise(windowsManager, windowConfig);
            }
        }
    }
}