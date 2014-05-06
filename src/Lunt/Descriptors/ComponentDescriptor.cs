using System;

namespace Lunt.Descriptors
{
    internal abstract class ComponentDescriptor
    {
        public string Name { get; set; }
        public abstract Type GetComponentType();
    }
}