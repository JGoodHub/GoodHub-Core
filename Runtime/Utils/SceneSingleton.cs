using UnityEngine;

namespace GoodHub.Core.Runtime
{
    public class SceneSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(T)) as T;

                    if (_instance == null)
                        Debug.LogWarning($"ERROR: No active instance of the LocalSingleton {typeof(T)} found in this scene");
                }

                return _instance;
            }
        }
    }
}
