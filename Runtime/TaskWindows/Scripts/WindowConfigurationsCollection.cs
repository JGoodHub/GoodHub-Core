using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime
{
    public class WindowConfigurationsCollection : ScriptableObject
    {
        [SerializeField] private List<WindowConfiguration> _windowConfigurations;

        public List<WindowConfiguration> WindowConfigurations => _windowConfigurations;
    }
}