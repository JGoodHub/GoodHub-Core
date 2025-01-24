using System;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [Serializable]
    public class ObservableBool
    {
        [SerializeField]
        private bool _value;

        public event Action<bool> OnValueChanged;

        public bool Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnValueChanged?.Invoke(_value);
                }
            }
        }

        public ObservableBool(bool initialValue)
        {
            _value = initialValue;
        }

        public void Flip()
        {
            Debug.LogError("FLIPPED");
            Value = !Value;
        }
        
        // Implicit conversion to & from bool
        public static implicit operator bool(ObservableBool observableBool) => observableBool._value;
        public static implicit operator ObservableBool(bool value) => new ObservableBool(value);

        // Overload && operator
        public static bool operator &(ObservableBool a, ObservableBool b) => a._value && b._value;
        public static bool operator &(ObservableBool a, bool b) => a._value && b;
        public static bool operator &(bool a, ObservableBool b) => a && b._value;

        // Overload || operator
        public static bool operator |(ObservableBool a, ObservableBool b) => a._value || b._value;
        public static bool operator |(ObservableBool a, bool b) => a._value || b;
        public static bool operator |(bool a, ObservableBool b) => a || b._value;
    }
}