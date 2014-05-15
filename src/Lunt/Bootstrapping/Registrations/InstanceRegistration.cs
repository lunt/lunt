using System;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// Represents an instance registration.
    /// </summary>
    public sealed class InstanceRegistration : ContainerRegistration
    {
        private readonly Type _registrationType;
        private readonly object _instance;

        /// <summary>
        ///  Gets the registration type.
        /// </summary>
        /// <value>The registration type.</value>
        public Type RegistrationType
        {
            get
            {
                return _registrationType;
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public object Instance
        {
            get
            {
                return _instance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="instance">The instance to register.</param>
        /// <param name="lifetime">The lifetime.</param>
        public InstanceRegistration(Type registrationType, object instance, Lifetime lifetime)
            : base(lifetime)
        {
            _registrationType = registrationType;
            _instance = instance;
        }
    }
}