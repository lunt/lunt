using System;
using System.Collections.Generic;

namespace Lunt.IO
{
    /// <summary>
    /// Defines methods to support the comparison of <see cref="Path"/> for equality.
    /// </summary>
    public sealed class PathComparer : IEqualityComparer<Path>
    {
        private readonly bool _isCaseSensitive;

        /// <summary>
        /// The default path comparer.
        /// </summary>        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly PathComparer Default = new PathComparer(Machine.IsUnix());

        /// <summary>
        /// Gets a value indicating whether the comparer is case sensitive.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the comparer is case sensitive; otherwise, <c>false</c>.
        /// </value>
        public bool IsCaseSensitive
        {
            get { return _isCaseSensitive; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathComparer" /> class.
        /// </summary>
        /// <param name="isCaseSensitive">if set to <c>true</c> the comparison will be case sensitive.</param>
        public PathComparer(bool isCaseSensitive)
        {
            _isCaseSensitive = isCaseSensitive;
        }

        /// <summary>
        /// Determines whether the specified paths are equal.
        /// </summary>
        /// <param name="x">An object to compare to y.</param>
        /// <param name="y">An object to compare to x.</param>
        /// <returns><c>true</c> if x and y refer to the same path; otherwise, <c>false</c>.</returns>
        public bool Equals(Path x, Path y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }

            if (IsCaseSensitive)
            {
                return x.FullPath.Equals(y.FullPath);
            }
            return x.FullPath.Equals(y.FullPath, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(Path obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (IsCaseSensitive)
            {
                return obj.FullPath.GetHashCode();
            }
            return obj.FullPath.ToUpperInvariant().GetHashCode();
        }
    }
}