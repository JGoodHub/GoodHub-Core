using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [NotNull]
    public abstract class Observable<T>
    {
        [NonSerialized]
        private T _value;

        /// <summary>
        /// Parameters are in the form of newValue, oldValue
        /// </summary>
        public event Action<T, T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value) == false)
                {
                    T oldValue = _value;
                    _value = value;
                    OnValueChanged?.Invoke(_value, oldValue);
                    Debug.LogError($"ValueChanged {_value} {oldValue}");
                }
            }
        }

        protected Observable(T initialValue, bool invokeChangedAction = true)
        {
            if (invokeChangedAction)
            {
                Value = initialValue;
            }
            else
            {
                _value = initialValue;
            }
        }

        public void Set(T value)
        {
            Value = value;
        }
    }
}