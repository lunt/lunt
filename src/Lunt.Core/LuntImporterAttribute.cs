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

namespace Lunt
{
    /// <summary>
    /// Provides properties that identify and provide metadata about the importer, such as supported file extensions and caching information.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LuntImporterAttribute : Attribute
    {
        private readonly string[] _fileExtensions;
        private Type _defaultProcessor;

        /// <summary>
        /// Gets or sets the type of the default processor for content read by this importer.
        /// </summary>
        /// <value>The default processor type.</value>
        public Type DefaultProcessor
        {
            get { return _defaultProcessor; }
            set { _defaultProcessor = value; }
        }

        /// <summary>
        /// Gets the supported file extensions of the importer.
        /// </summary>
        /// <value>The file extensions.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
        public string[] FileExtensions
        {
            get { return _fileExtensions; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntImporterAttribute" /> class.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        public LuntImporterAttribute(string fileExtension)
        {
            _fileExtensions = new[] {fileExtension};
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LuntImporterAttribute" /> class.
        /// </summary>
        /// <param name="fileExtensions">The extensions.</param>
        public LuntImporterAttribute(params string[] fileExtensions)
        {
            _fileExtensions = fileExtensions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntImporterAttribute" /> class.
        /// </summary>
        /// <param name="fileExtensions">The extensions.</param>
        /// <param name="defaultProcessor">The default processor.</param>
        public LuntImporterAttribute(string[] fileExtensions, Type defaultProcessor)
        {
            _fileExtensions = fileExtensions;
            _defaultProcessor = defaultProcessor;
        }
    }
}