using System;
using UnityEngine;

namespace GoodHub.Core.Runtime
{
    public class GlobalSingleton<T> : SceneSingleton<T> where T : MonoBehaviour
    {
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
