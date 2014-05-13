using System;

namespace Lunt.IO
{
    /// <summary>
    /// Represents a path.
    /// </summary>
    public abstract class Path
    {
        private readonly string _path;
        private readonly bool _isRelative;
        private readonly string[] _segments;

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <value>The full path.</value>
        public string FullPath
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets a value indicating whether the path is relative.
        /// </summary>
        /// <value>
        /// <c>true</c> if the path is relative; otherwise, <c>false</c>.
        /// </value>
        public bool IsRelative
        {
            get { return _isRelative; }
        }

        /// <summary>
        /// Gets the segments that make up the path.
        /// </summary>
        /// <value>The segments that make up the path.</value>
        public string[] Segments
        {
            get { return _segments; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Path" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <exception cref="System.ArgumentNullException">path</exception>
        /// <exception cref="System.ArgumentException">Path cannot be empty.</exception>
        protected Path(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path cannot be empty.", "path");
            }
            _path = path.Replace('\\', '/').Trim();
            _path = _path == "./" ? string.Empty : _path;

            // Remove relative part of a path.
            if (_path.StartsWith("./", StringComparison.Ordinal))
            {
                _path = _path.Substring(2);
            }

            _isRelative = !System.IO.Path.IsPathRooted(_path);

            // Extract path segments.
            _segments = _path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents the path.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this path.
        /// </returns>
        public override string ToString()
        {
            return FullPath;
        }
    }
}