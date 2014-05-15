using System;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// Represents a type registration.
    /// </summary>
    public sealed class TypeRegistration : ContainerRegistration
    {
        private readonly Type _registrationType;
        private readonly Type _implementationType;

        /// <summary>
        /// Gets the registration type.
        /// </summary>
        /// <value>The registration type.</value>
        public Type RegistrationType
        {
            get { return _registrationType; }
        }

        /// <summary>
        /// Gets the implementing type.
        /// </summary>
        /// <value>The type of the implementation.</value>
        public Type ImplementationType
        {
            get { return _implementationType; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="lifetime">The lifetime.</param>
        public TypeRegistration(Type registrationType, Lifetime lifetime)
            : this(registrationType, registrationType, lifetime)
        {            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="implementationType">The implementation type.</param>
        /// <param name="lifetime">The lifetime.</param>
        public TypeRegistration(Type registrationType, Type implementationType, Lifetime lifetime)
            : base(lifetime)
        {
            _registrationType = registrationType;
            _implementationType = implementationType;
        }
    }
}