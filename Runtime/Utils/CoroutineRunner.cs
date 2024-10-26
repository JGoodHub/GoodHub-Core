using System.Collections;
using UnityEngine;

namespace GoodHub.Core.Runtime.Utils
{
    public class CoroutineRunner : GlobalSingleton<CoroutineRunner>
    {
        /// <summary>
        /// Starts a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to start.</param>
        public static void RunCoroutine(IEnumerator coroutine)
        {
            Singleton.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Stops a coroutine.
        /// </summary>
        /// <param name="coroutine">The coroutine to stop.</param>
        public static void HaltCoroutine(IEnumerator coroutine)
        {
            Singleton.StopCoroutine(coroutine);
        }

        /// <summary>
        /// Stops all running coroutines.
        /// </summary>
        public static void HaltAllCoroutines()
        {
            Singleton.StopAllCoroutines();
        }
    }
}