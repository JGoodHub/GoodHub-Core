using System;
using JetBrains.Annotations;

namespace GoodHub.Core.Runtime.Observables
{
    public interface IObservable
    {
        public string StringValue { get; }
    }

    [NotNull]
    public abstract class Observable<T>
    {
        [NonSerialized]
        private T _value;

        /// <summary>
        /// Parameters are in the form of oldValue, newValue
        /// </summary>
        public event Action<T, T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (Equals(_value, value))
                    return;

                T oldValue = _value;
                _value = value;
                OnValueChanged?.Invoke(oldValue, _value);
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
