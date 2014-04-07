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
using System.Collections.Generic;
using System.Reflection;
using Lunt.Diagnostics;

namespace Lunt.Runtime
{
    using Lunt.Diagnostics;

    /// <summary>
    /// Provides a mechanism for finding pipeline components within an assembly.
    /// </summary>
    public sealed class PipelineAssemblyScanner : IPipelineScanner
    {
        private readonly AssemblyScanner _scanner;
        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineAssemblyScanner" /> class.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="assembly">The assembly.</param>
        public PipelineAssemblyScanner(IBuildLog log, Assembly assembly)
        {
            _scanner = new AssemblyScanner(log);
            _assembly = assembly;
        }

        /// <summary>
        /// Performs a scan for components.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IPipelineComponent> Scan()
        {
            return _scanner.Scan<IPipelineComponent>(_assembly, log: true);
        }
    }
}