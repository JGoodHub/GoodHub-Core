using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime
{

    // [CreateAssetMenu(fileName = "FileName", menuName = "Scriptable Singletons/Create New FileName")]
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {

        private static T _singleton;

        public static T Singleton
        {
            get
            {
                _singleton ??= Resources.Load($"Singletons/{typeof(T).Name}") as T;

                if (_singleton == null)
                    throw new Exception($"Exception: No instance could be found for the singleton {typeof(T)}. Check an instance of the scriptable object has been created and has been placed inside a Resources/Singletons folder.");

                return _singleton;
            }
        }

    }


}
