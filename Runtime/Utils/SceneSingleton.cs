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
                    _instance = FindObjectOfType<T>(true);

                    if (_instance == null)
                        Debug.LogWarning($"ERROR: No active instance of the Singleton {typeof(T)} found in this scene");
                    else if (_instance.GetType() == typeof(GlobalSingleton<T>))
                        DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

    }

}