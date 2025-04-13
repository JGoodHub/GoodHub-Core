using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [Serializable]
    [NotNull]
    public class ObservableString : Observable<string>
    {
        public ObservableString(string initialValue, bool invokeChangedAction = true) : base(initialValue,
            invokeChangedAction) { }

        // Implicit conversion
        public static implicit operator string(ObservableString o)
        {
            if (o == null)
            {
                Debug.LogError("Observable is null, cannot complete implicit conversion. Returning default value.");
                return string.Empty;
            }

            return o.Value;
        }

        //Override Equals and GetHashCode
        public override bool Equals(object obj)
        {
            return obj is ObservableBool other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}