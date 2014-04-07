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
using System.Linq;

namespace Lunt
{
    /// <summary>
    /// Represents asset metadata.
    /// </summary>
    public sealed class AssetMetadata
    {
        private readonly Dictionary<string, string> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetMetadata" /> class.
        /// </summary>
        /// <param name="dictionary">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">dictionary</exception>
        public AssetMetadata(IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            _dictionary = new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the number of metadata items.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// Determines whether the specified key is defined.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is defined; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDefined(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <returns>The keys.</returns>
        public string[] GetKeys()
        {
            return _dictionary.Keys.ToArray();
        }

        /// <summary>
        /// Gets the value for a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public string GetValue(string key)
        {
            if (this.IsDefined(key))
            {
                return _dictionary[key];
            }
            return null;
        }
    }
}