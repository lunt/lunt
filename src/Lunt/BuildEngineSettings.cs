using System;
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// The build engine settings.
    /// </summary>
    public sealed class BuildEngineSettings
    {
        private readonly FilePath _buildConfigurationPath;

        /// <summary>
        /// Gets the build configuration path.
        /// </summary>
        /// <value>The build configuration path.</value>
        public FilePath BuildConfigurationPath
        {
            get { return _buildConfigurationPath; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildEngineSettings"/> class.
        /// </summary>
        /// <param name="buildConfigurationPath">The build configuration path.</param>
        public BuildEngineSettings(FilePath buildConfigurationPath)
        {
            if (buildConfigurationPath == null)
            {
                throw new ArgumentNullException("buildConfigurationPath");
            }
            _buildConfigurationPath = buildConfigurationPath;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to build incrementally.
        /// </summary>
        /// <value>
        ///   <c>true</c> if building incrementally; otherwise, <c>false</c>.
        /// </value>
        public bool Incremental { get; set; }

        /// <summary>
        /// Gets or sets the input path.
        /// </summary>
        /// <value>The input path.</value>
        public DirectoryPath InputPath { get; set; }

        /// <summary>
        /// Gets or sets the output path.
        /// </summary>
        /// <value>The output path.</value>
        public DirectoryPath OutputPath { get; set; }
    }
}