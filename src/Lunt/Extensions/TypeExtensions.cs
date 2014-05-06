using System;
using System.Diagnostics;

namespace Lunt
{
    internal static class TypeExtensions
    {
        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            Debug.Assert(type != null);
            var attributes = type.GetCustomAttributes(typeof (T), false);
            return (attributes.Length > 0) ? (T) attributes[0] : null;
        }
    }
}