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

        public static Tweener Float(float from, float to, float duration, Action<float> stepCallback)
        {
            Tweener tweener = new Tweener(stepCallback);
            _routineRunner.StartCoroutine(LerpRoutine(from, to, duration, tweener, 0f));
            return tweener;
        }

        public static Tweener DelayedFloat(float from, float to, float duration, Action<float> stepCallback, float delay)
        {
            Tweener tweener = new Tweener(stepCallback);
            _routineRunner.StartCoroutine(LerpRoutine(from, to, duration, tweener, delay));
            return tweener;
        }

        public static Tweener Vector2(Vector2 from, Vector2 to, float duration, Action<Vector2> stepCallback)
        {
            Tweener tweener = new Tweener(t =>
            {
                Vector2 lerpVector = UnityEngine.Vector2.Lerp(from, to, t);
                stepCallback?.Invoke(lerpVector);
            });

            _routineRunner.StartCoroutine(LerpRoutine(0f, 1f, duration, tweener, 0f));

            return tweener;
        }

        private static IEnumerator LerpRoutine(float from, float to, float duration, Tweener tweener, float delay)
        {
            if (delay > 0f)
                yield return new WaitForSeconds(delay);

            tweener.StartedCallback?.Invoke();

            float delta = to - from;

            duration = duration < 0f ? 0f : duration;
            float inverseDuration = 1f / duration;
            float time = 0f;

            float lastFrameTime = -1f;

            while (time < duration)
            {
                float progress = time * inverseDuration;
                float easedProgress = EasingUtil.Ease(progress, tweener.Easing);
                float interpolate = from + (delta * easedProgress);

                try
                {
                    tweener.SteppedCallback?.Invoke(interpolate);

                    if (tweener.TriggerCallbacks.Count > 0)
                    {
                        foreach (float boundary in tweener.TriggerCallbacks.Keys)
                        {
                            if (lastFrameTime <= boundary && time >= boundary)
                            {
                                tweener.TriggerCallbacks[boundary]?.Invoke();
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
                tweener.SteppedCallback?.Invoke(to);

                tweener.CompletedCallback?.Invoke();

                if (tweener.TriggerCallbacks.Count > 0 && tweener.TriggerCallbacks.TryGetValue(1f, out Action callback))
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

    public class Tweener
    {
        private Action _startedCallback;
        private Action<float> _steppedCallback;
        private Action _completedCallback;
        private Dictionary<float, Action> _triggerCallbacks = new Dictionary<float, Action>();

        private Easing _easing;

        public Action StartedCallback => _startedCallback;

        public Action<float> SteppedCallback => _steppedCallback;

        public Action CompletedCallback => _completedCallback;

        public Dictionary<float, Action> TriggerCallbacks => _triggerCallbacks;

        public Easing Easing => _easing;

        public Tweener(Action<float> steppedCallback)
        {
            _steppedCallback = steppedCallback;
        }

        public Tweener OnStarted(Action callback)
        {
            _startedCallback = callback;
            return this;
        }

        public Tweener OnCompleted(Action callback)
        {
            _completedCallback = callback;
            return this;
        }

        public Tweener AddTrigger(float value, Action callback)
        {
            _triggerCallbacks[value] = callback;
            return this;
        }

        public Tweener SetEasing(Easing easing)
        {
            _easing = easing;
            return this;
        }

        public void Run()
        {
        }
    }

}