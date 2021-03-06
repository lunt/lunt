﻿using System;
using System.Collections.Generic;
using Lunt.Diagnostics;
using Lunt.IO;
using Lunt.Runtime;

namespace Lunt.Bootstrapping
{
    /// <summary>
    /// The default internal configuration that makes it easy to configure
    /// what components a build kernel should be made of.
    /// </summary>
    public sealed class InternalConfiguration : IInternalConfiguration
    {
        /// <summary>
        /// Gets or sets the build environment type to be used.
        /// </summary>
        /// <value>
        /// The build environment type to be used.
        /// </value>
        public Type BuildEnvironment { get; set; }

        /// <summary>
        /// Gets or sets the pipeline scanner type to be used.
        /// </summary>
        /// <value>
        /// The pipeline scanner type to be used.
        /// </value>
        public Type PipelineScanner { get; set; }

        /// <summary>
        /// Gets or sets the file system type to be used.
        /// </summary>
        /// <value>
        /// The file system type to be used.
        /// </value>
        public Type FileSystem { get; set; }

        /// <summary>
        /// Gets or sets the hash computer type to be used.
        /// </summary>
        /// <value>
        /// The hash computer type to be used.
        /// </value>
        public Type HashComputer { get; set; }

        /// <summary>
        /// Gets or sets the build log type to be used.
        /// </summary>
        /// <value>The build log type to be used.</value>
        public Type BuildLog { get; set; }

        /// <summary>
        /// Gets or sets the build kernel type to be used.
        /// </summary>
        /// <value>The build kernel type to be used.</value>
        public Type BuildKernel { get; set; }

        /// <summary>
        /// Gets or sets the build configuration reader type to be used.
        /// </summary>
        /// <value>The build configuration reader type to be used.</value>
        public Type BuildConfigurationReader { get; set; }

        /// <summary>
        /// Gets or sets the build manifest provider type to be used.
        /// </summary>
        /// <value>The build manifest provider type to be used.</value>
        public Type BuildManifestProvider { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalConfiguration"/> class.
        /// </summary>
        public InternalConfiguration()
        {
            BuildEnvironment = typeof(BuildEnvironment);
            PipelineScanner = typeof(AppDomainScanner);
            FileSystem = typeof(FileSystem);
            HashComputer = typeof(HashComputer);
            BuildLog = typeof(TraceBuildLog);
            BuildKernel = typeof(BuildKernel);
            BuildConfigurationReader = typeof(BuildConfigurationReader);
            BuildManifestProvider = typeof(BuildManifestProvider);
        }

        /// <summary>
        /// Gets all registrations.
        /// </summary>
        /// <returns>The registrations.</returns>
        public IEnumerable<ContainerRegistration> GetRegistrations()
        {
            return new List<ContainerRegistration>
            {
                new TypeRegistration(typeof(IBuildEnvironment), BuildEnvironment, Lifetime.Singleton),
                new TypeRegistration(typeof(IPipelineScanner), PipelineScanner, Lifetime.Singleton),
                new TypeRegistration(typeof(IFileSystem), FileSystem, Lifetime.Singleton),
                new TypeRegistration(typeof(IHashComputer), HashComputer, Lifetime.Singleton),
                new TypeRegistration(typeof(IBuildLog), BuildLog, Lifetime.Singleton),
                new TypeRegistration(typeof(IBuildKernel), BuildKernel, Lifetime.Singleton),
                new TypeRegistration(typeof(IBuildKernel), BuildKernel, Lifetime.Singleton),
                new TypeRegistration(typeof(IBuildConfigurationReader), BuildConfigurationReader, Lifetime.Singleton),
                new TypeRegistration(typeof(IBuildManifestProvider), BuildManifestProvider, Lifetime.Singleton)
            };
        }
    }
}
