using System;
using Lunt.Bootstrapping;
using Lunt.Extensions;

namespace Lunt
{
    /// <summary>
    /// The build engine.
    /// </summary>
    public sealed class BuildEngine
    {
        private readonly IBootstrapper _bootstrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEngine"/> class.
        /// </summary>
        public BuildEngine()
            : this((DefaultBootstrapper)null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEngine"/> class.
        /// </summary>
        /// <param name="configuration">The internal configuration.</param>
        public BuildEngine(IInternalConfiguration configuration)
            : this(new DefaultBootstrapper(configuration))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEngine"/> class.
        /// </summary>
        /// <param name="bootstrapper">The bootstrapper.</param>
        public BuildEngine(IBootstrapper bootstrapper)
        {
            _bootstrapper = bootstrapper ?? new DefaultBootstrapper();
        }

        /// <summary>
        /// Executes a build with the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The build manifest that is the result of the build.</returns>
        public BuildManifest Build(BuildEngineSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var environment = _bootstrapper.GetBuildEnvironment();
            var workingDirectory = environment.GetWorkingDirectory();

            // Get the build configuration path.
            var buildConfigurationPath = settings.BuildConfigurationPath;
            if (buildConfigurationPath.IsRelative)
            {
                buildConfigurationPath = workingDirectory.Combine(buildConfigurationPath);
            }

            // Read and fix the build configuration.
            var reader = _bootstrapper.GetBuildConfigurationReader();
            var configuration = reader.Read(buildConfigurationPath);
            configuration.Incremental = settings.Incremental;
            configuration.InputDirectory = settings.InputPath;
            configuration.OutputDirectory = settings.OutputPath;

            // Set default directories.
            if (configuration.InputDirectory == null)
            {
                configuration.InputDirectory = buildConfigurationPath.GetDirectory();
            }
            if (configuration.OutputDirectory == null)
            {
                configuration.OutputDirectory = "Output";
            }

            // Make relative paths absolute.
            if (configuration.InputDirectory.IsRelative)
            {
                configuration.InputDirectory = workingDirectory.Combine(configuration.InputDirectory);
            }
            if (configuration.OutputDirectory.IsRelative)
            {
                configuration.OutputDirectory = workingDirectory.Combine(configuration.OutputDirectory);
            }

            // TODO: Load previous manifest.
            var manifestProvider = _bootstrapper.GetService<IBuildManifestProvider>();
            var manifestPath = buildConfigurationPath.ChangeExtension(".manifest");
            var previousManifest = manifestProvider.LoadManifest(environment.FileSystem, manifestPath);

            // Build the configuration and return the result.
            using (var engine = _bootstrapper.GetService<IBuildKernel>())
            {
                var manifest = engine.Build(configuration, previousManifest);

                // TODO: Save manifest.
                manifestProvider.SaveManifest(environment.FileSystem, manifestPath, manifest);

                return manifest;
            }
        }

    }
}
