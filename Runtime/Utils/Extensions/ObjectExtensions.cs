using System;

namespace GoodHub.Core.Runtime.Utils
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Attempts to cast the specified object to the specified reference type.
        /// Returns null if the cast is not successful.
        /// </summary>
        /// <typeparam name="T">The type to cast the object to. Must be a reference type.</typeparam>
        /// <param name="obj">The object to cast.</param>
        /// <returns>The object cast to type <typeparamref name="T"/>, or null if the cast fails.</returns>
        public static T As<T>(this object obj) where T : class
        {
            return obj as T;
        }
    }
}