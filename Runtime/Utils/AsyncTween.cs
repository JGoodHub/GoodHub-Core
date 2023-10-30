using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GoodHub.Core.Runtime.Utils
{

    public static class AsyncTween
    {
        private static AsyncRoutineRunner _routineRunner;

        static AsyncTween()
        {
            _routineRunner = new GameObject("[AsyncTween_RoutineRunner]").AddComponent<AsyncRoutineRunner>();
            Object.DontDestroyOnLoad(_routineRunner.gameObject);
        }

        public static void Lerp(float from, float to, float duration, Action<float> stepCallback, Action completeCallback = null, Dictionary<float, Action> timeCallbacks = null, float delay = 0f)
        {
            _routineRunner.StartCoroutine(LerpRoutine(from, to, duration, stepCallback, completeCallback, timeCallbacks, delay));
        }

        private static IEnumerator LerpRoutine(float from, float to, float duration, Action<float> stepCallback, Action completeCallback, Dictionary<float, Action> timeCallbacks, float delay)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            float delta = to - from;

            duration = duration < 0f ? 0f : duration;
            float inverseDuration = 1f / duration;
            float time = 0f;

            float lastFrameTime = -1f;

            while (time < duration)
            {
                float progress = time * inverseDuration;
                float interpolate = from + (delta * progress);

                try
                {
                    stepCallback?.Invoke(interpolate);

                    if (timeCallbacks != null)
                    {
                        foreach (float boundary in timeCallbacks.Keys)
                        {
                            if (lastFrameTime <= boundary && time >= boundary)
                            {
                                timeCallbacks[boundary]?.Invoke();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"[AsyncLerp]: Exception was handled inside AsyncLerp (progress: {progress}, lerp: {interpolate})");
                    Debug.LogError(e);
                    throw;
                }

                yield return null;

                lastFrameTime = time;
                time += Time.deltaTime;
            }

            try
            {
                stepCallback?.Invoke(to);

                completeCallback?.Invoke();

                if (timeCallbacks != null && timeCallbacks.TryGetValue(1f, out Action callback))
                    callback?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"[AsyncLerp]: Exception was handled inside AsyncLerp (progress: {duration}, lerp: {to})");
                Debug.LogError(e);
                throw;
            }
        }
    }

}