// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
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

            if (_isCaseSensitive)
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
            if (_isCaseSensitive)
            {
                return obj.FullPath.GetHashCode();
            }
            return obj.FullPath.ToUpperInvariant().GetHashCode();
        }
    }
}