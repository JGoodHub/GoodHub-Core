﻿using System;
using JetBrains.Annotations;
using UnityEngine;

namespace GoodHub.Core.Runtime.Observables
{
    [Serializable]
    [NotNull]
    public class ObservableEnum<TEnum> : Observable<TEnum>, IObservable
        where TEnum : Enum
    {
        public ObservableEnum(TEnum initialValue, bool invokeChangedAction = true)
            : base(initialValue, invokeChangedAction) { }

        public int IntValue => Convert.ToInt32(Value);

        public string StringValue => Value.ToString();

        // Implicit conversion
        public static implicit operator TEnum(ObservableEnum<TEnum> o)
        {
            if (o == null)
            {
                Debug.LogError(
                    "Observable is null, cannot complete implicit conversion. Returning default value."
                );
                return default;
            }

            return o.Value;
        }

        //Override Equals and GetHashCode
        public override bool Equals(object obj)
        {
            return obj is ObservableEnum<TEnum> other && Equals(Value, other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
