using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [Serializable]
    [NotNull]
    public class ObservableBool : Observable<bool>
    {
        public ObservableBool(bool initialValue, bool invokeChangedAction = true) : base(initialValue, invokeChangedAction) { }

        // Implicit conversion
        public static implicit operator bool(ObservableBool o)
        {
            if (o == null)
            {
                Debug.LogError("Observable is null, cannot complete implicit conversion. Returning default value.");
                return false;
            }

            return o.Value;
        }

        // public static implicit operator ObservableBool(bool value) => new ObservableBool(value);

        // Comparison operators
        // public static bool operator ==(ObservableBool a, ObservableBool b) => a!.Value == b!.Value;
        // public static bool operator ==(ObservableBool a, bool b) => a!.Value == b;
        // public static bool operator ==(bool a, ObservableBool b) => a == b!.Value;
        //
        // public static bool operator !=(ObservableBool a, ObservableBool b) => a!.Value != b!.Value;
        // public static bool operator !=(ObservableBool a, bool b) => a!.Value != b;
        // public static bool operator !=(bool a, ObservableBool b) => a != b!.Value;
        //
        // public static bool operator &(ObservableBool a, ObservableBool b) => a.Value && b.Value;
        // public static bool operator &(ObservableBool a, bool b) => a.Value && b;
        // public static bool operator &(bool a, ObservableBool b) => a && b.Value;
        //
        // public static bool operator |(ObservableBool a, ObservableBool b) => a.Value || b.Value;
        // public static bool operator |(ObservableBool a, bool b) => a.Value || b;
        // public static bool operator |(bool a, ObservableBool b) => a || b.Value;

        //Override Equals and GetHashCode
        public override bool Equals(object obj)
        {
            return obj is ObservableBool other && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public void Flip()
        {
            Value = !Value;
        }
    }
}