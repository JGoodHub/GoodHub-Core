using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [Serializable]
    [NotNull]
    public class ObservableInt : Observable<int>
    {
        public ObservableInt(int initialValue, bool invokeChangedAction = true) : base(initialValue,
            invokeChangedAction) { }

        // Implicit conversion
        public static implicit operator int(ObservableInt o)
        {
            if (o == null)
            {
                Debug.LogError("Observable is null, cannot complete implicit conversion. Returning default value.");
                return 0;
            }

            return o.Value;
        }

        // public static implicit operator ObservableInt(int value) => new ObservableInt(value);

        // Arithmetic operators
        // public static int operator +(ObservableInt a, ObservableInt b) => a.Value + b.Value;
        // public static int operator +(ObservableInt a, int b) => a.Value + b;
        // public static int operator +(int a, ObservableInt b) => a + b.Value;
        //
        // public static int operator -(ObservableInt a, ObservableInt b) => a.Value - b.Value;
        // public static int operator -(ObservableInt a, int b) => a.Value - b;
        // public static int operator -(int a, ObservableInt b) => a - b.Value;
        //
        // public static int operator *(ObservableInt a, ObservableInt b) => a.Value * b.Value;
        // public static int operator *(ObservableInt a, int b) => a.Value * b;
        // public static int operator *(int a, ObservableInt b) => a * b.Value;
        //
        // public static int operator /(ObservableInt a, ObservableInt b) => a.Value / b.Value;
        // public static int operator /(ObservableInt a, int b) => a.Value / b;
        // public static int operator /(int a, ObservableInt b) => a / b.Value;
        //
        // public static int operator %(ObservableInt a, ObservableInt b) => a.Value % b.Value;
        // public static int operator %(ObservableInt a, int b) => a.Value % b;
        // public static int operator %(int a, ObservableInt b) => a % b.Value;

        // Comparison operators
        // public static bool operator ==(ObservableInt a, ObservableInt b) => a!.Value == b!.Value;
        // public static bool operator ==(ObservableInt a, int b) => a!.Value == b;
        // public static bool operator ==(int a, ObservableInt b) => a == b!.Value;
        //
        // public static bool operator !=(ObservableInt a, ObservableInt b) => a!.Value != b!.Value;
        // public static bool operator !=(ObservableInt a, int b) => a!.Value != b;
        // public static bool operator !=(int a, ObservableInt b) => a != b!.Value;
        //
        // public static bool operator >(ObservableInt a, ObservableInt b) => a.Value > b.Value;
        // public static bool operator >(ObservableInt a, int b) => a.Value > b;
        // public static bool operator >(int a, ObservableInt b) => a > b.Value;
        //
        // public static bool operator <(ObservableInt a, ObservableInt b) => a.Value < b.Value;
        // public static bool operator <(ObservableInt a, int b) => a.Value < b;
        // public static bool operator <(int a, ObservableInt b) => a < b.Value;
        //
        // public static bool operator >=(ObservableInt a, ObservableInt b) => a.Value >= b.Value;
        // public static bool operator >=(ObservableInt a, int b) => a.Value >= b;
        // public static bool operator >=(int a, ObservableInt b) => a >= b.Value;
        //
        // public static bool operator <=(ObservableInt a, ObservableInt b) => a.Value <= b.Value;
        // public static bool operator <=(ObservableInt a, int b) => a.Value <= b;
        // public static bool operator <=(int a, ObservableInt b) => a <= b.Value;

        //Override Equals and GetHashCode
        public override bool Equals(object obj)
        {
            return obj is ObservableInt other && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}