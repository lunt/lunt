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
using Lunt.IO;

namespace Lunt
{
    /// <summary>
    /// Provides a base class to use when developing custom importer components.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LuntImporter<T> : ILuntImporter
    {
        /// <summary>
        /// Imports an asset from the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The asset file to import.</param>
        /// <returns></returns>
        public abstract T Import(LuntContext context, IFile file);

        object ILuntImporter.Import(LuntContext context, IFile file)
        {
            return this.Import(context, file);
        }
    }
}