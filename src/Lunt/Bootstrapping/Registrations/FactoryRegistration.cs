﻿using System;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// Represents a factory registration.
    /// </summary>
    public sealed class FactoryRegistration : ContainerRegistration
    {
        private readonly Func<FactoryRegistrationContext, object> _factory;

        internal Func<FactoryRegistrationContext, object> Factory
        {
            get { return _factory; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryRegistration"/> class.
        /// </summary>
        /// <param name="registrationType">The registration type.</param>
        /// <param name="factory">The factory delegate.</param>
        public FactoryRegistration(Type registrationType, Func<FactoryRegistrationContext, object> factory)
            : base(registrationType, Lifetime.Default)
        {
            _factory = factory;
        }
    }
}
