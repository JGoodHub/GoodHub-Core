using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GoodHub.Core.Runtime
{

    // [CreateAssetMenu(fileName = "FileName", menuName = "Scriptable Singletons/Create New FileName")]
    public class SingletonScriptable<T> : ScriptableObject where T : ScriptableObject
    {

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load($"Singletons/{typeof(T).Name}") as T;

                if (_instance == null)
                    throw new Exception($"Exception: No instance could be found for the singleton {typeof(T)}. Check an instance of the scriptable object has been created and has been placed inside a Resources/Singletons folder.");

                return _instance;
            }
        }

    }
    
    

}