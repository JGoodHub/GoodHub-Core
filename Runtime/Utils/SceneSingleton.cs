using UnityEngine;

namespace GoodHub.Core.Runtime
{
    public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _singleton;

        public static T Singleton
        {
            get
            {
                if (_singleton != null)
                    return _singleton;
                
                _singleton = FindObjectOfType<T>(true);

                if (_singleton == null)
                    Debug.LogError($"ERROR: No active instance of the Singleton {typeof(T)} found in this scene");
                else if (_singleton.GetType() == typeof(GlobalSingleton<T>))
                    DontDestroyOnLoad(_singleton);

                return _singleton;
            }
        }
    }
}