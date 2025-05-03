using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [Serializable]
    [NotNull]
    public class ObservableObject<TObject> : Observable<TObject>, IObservable
        where TObject : class
    {
        public ObservableObject(TObject initialValue, bool invokeChangedAction = true)
            : base(initialValue, invokeChangedAction) { }

        public string StringValue =>
            $"{(Value == null ? "null" : Value.ToString())} ({typeof(TObject).Name})";

        // Implicit conversion
        public static implicit operator TObject(ObservableObject<TObject> o)
        {
            if (o == null)
            {
                Debug.LogError(
                    "Observable is null, cannot complete implicit conversion. Returning default value."
                );
                return null;
            }

            return o.Value;
        }

        //Override Equals and GetHashCode
        public override bool Equals(object obj)
        {
            return obj is ObservableObject<TObject> other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
