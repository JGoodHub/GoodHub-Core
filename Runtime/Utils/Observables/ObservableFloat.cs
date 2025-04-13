using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [Serializable]
    [NotNull]
    public class ObservableFloat : Observable<float>
    {
        public ObservableFloat(float initialValue, bool invokeChangedAction = true) : base(initialValue,
            invokeChangedAction) { }

        // Implicit conversion
        public static implicit operator float(ObservableFloat o)
        {
            if (o == null)
            {
                Debug.LogError("Observable is null, cannot complete implicit conversion. Returning default value.");
                return 0f;
            }

            return o.Value;
        }

        // public static implicit operator ObservableFloat(float value) => new ObservableFloat(value);

        // Arithmetic operators
        // public static float operator +(ObservableFloat a, ObservableFloat b) => a.Value + b.Value;
        // public static float operator +(ObservableFloat a, float b) => a.Value + b;
        // public static float operator +(float a, ObservableFloat b) => a + b.Value;
        //
        // public static float operator -(ObservableFloat a, ObservableFloat b) => a.Value - b.Value;
        // public static float operator -(ObservableFloat a, float b) => a.Value - b;
        // public static float operator -(float a, ObservableFloat b) => a - b.Value;
        //
        // public static float operator *(ObservableFloat a, ObservableFloat b) => a.Value * b.Value;
        // public static float operator *(ObservableFloat a, float b) => a.Value * b;
        // public static float operator *(float a, ObservableFloat b) => a * b.Value;
        //
        // public static float operator /(ObservableFloat a, ObservableFloat b) => a.Value / b.Value;
        // public static float operator /(ObservableFloat a, float b) => a.Value / b;
        // public static float operator /(float a, ObservableFloat b) => a / b.Value;
        //
        // public static float operator %(ObservableFloat a, ObservableFloat b) => a.Value % b.Value;
        // public static float operator %(ObservableFloat a, float b) => a.Value % b;
        // public static float operator %(float a, ObservableFloat b) => a % b.Value;

        // Comparison operators
        // public static bool operator ==(ObservableFloat a, ObservableFloat b) => a!.Value == b!.Value;
        // public static bool operator ==(ObservableFloat a, float b) => a!.Value == b;
        // public static bool operator ==(float a, ObservableFloat b) => a == b!.Value;
        //
        // public static bool operator !=(ObservableFloat a, ObservableFloat b) => a!.Value != b!.Value;
        // public static bool operator !=(ObservableFloat a, float b) => a!.Value != b;
        // public static bool operator !=(float a, ObservableFloat b) => a != b!.Value;
        //
        // public static bool operator >(ObservableFloat a, ObservableFloat b) => a.Value > b.Value;
        // public static bool operator >(ObservableFloat a, float b) => a.Value > b;
        // public static bool operator >(float a, ObservableFloat b) => a > b.Value;
        //
        // public static bool operator <(ObservableFloat a, ObservableFloat b) => a.Value < b.Value;
        // public static bool operator <(ObservableFloat a, float b) => a.Value < b;
        // public static bool operator <(float a, ObservableFloat b) => a < b.Value;
        //
        // public static bool operator >=(ObservableFloat a, ObservableFloat b) => a.Value >= b.Value;
        // public static bool operator >=(ObservableFloat a, float b) => a.Value >= b;
        // public static bool operator >=(float a, ObservableFloat b) => a >= b.Value;
        //
        // public static bool operator <=(ObservableFloat a, ObservableFloat b) => a.Value <= b.Value;
        // public static bool operator <=(ObservableFloat a, float b) => a.Value <= b;
        // public static bool operator <=(float a, ObservableFloat b) => a <= b.Value;

        //Override Equals and GetHashCode
        public override bool Equals(object obj)
        {
            return obj is ObservableFloat other && Mathf.Approximately(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}