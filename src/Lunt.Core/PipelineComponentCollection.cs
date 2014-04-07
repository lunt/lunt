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
using System.Collections;
using System.Collections.Generic;
using Lunt.Runtime;

namespace Lunt
{
    using Lunt.Runtime;

    /// <summary>
    /// Provides a mechanism for enumerating Lunt components.
    /// </summary>
    public sealed class PipelineComponentCollection : IPipelineComponentCollection
    {
        private readonly List<IPipelineComponent> _components;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineComponentCollection" /> class.
        /// </summary>
        /// <param name="components">The components.</param>
        public PipelineComponentCollection(IEnumerable<IPipelineComponent> components)
        {
            _components = new List<IPipelineComponent>(components);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineComponentCollection" /> class.
        /// </summary>
        /// <param name="scanner">The scanner.</param>
        public PipelineComponentCollection(IPipelineScanner scanner)
        {
            _components = new List<IPipelineComponent>(scanner.Scan());
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{ILuntComponent}" /> object that can be used to iterate through the collection.</returns>
        public IEnumerator<IPipelineComponent> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}