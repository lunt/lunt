using System;
using System.Diagnostics.CodeAnalysis;

namespace Lunt
{
    /// <summary>
    /// Provides properties that identify and provide metadata about the importer, such as supported file extensions and caching information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class ImporterAttribute : Attribute
    {
        private readonly string[] _fileExtensions;

        /// <summary>
        /// Gets or sets the type of the default processor for content read by this importer.
        /// </summary>
        /// <value>The default processor type.</value>
        public Type DefaultProcessor { get; set; }

        /// <summary>
        /// Gets the supported file extensions of the importer.
        /// </summary>
        /// <value>The file extensions.</value>
        [SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] FileExtensions
        {
            get { return _fileExtensions; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterAttribute" /> class.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        public ImporterAttribute(string fileExtension)
        {
            _fileExtensions = new[] {fileExtension};
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterAttribute" /> class.
        /// </summary>
        /// <param name="fileExtensions">The extensions.</param>
        public ImporterAttribute(params string[] fileExtensions)
        {
            _fileExtensions = fileExtensions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterAttribute" /> class.
        /// </summary>
        /// <param name="fileExtensions">The extensions.</param>
        /// <param name="defaultProcessor">The default processor.</param>
        public ImporterAttribute(string[] fileExtensions, Type defaultProcessor)
        {
            _fileExtensions = fileExtensions;
            DefaultProcessor = defaultProcessor;
        }
    }
}