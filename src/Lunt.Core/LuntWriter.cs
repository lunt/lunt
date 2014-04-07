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
using Lunt.IO;

namespace Lunt
{
    using Lunt.IO;

    /// <summary>
    /// Provides a base class to use when developing custom writer components.
    /// </summary>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public abstract class LuntWriter<TTarget> : ILuntWriter
    {
        /// <summary>
        /// Compiles an object.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public abstract void Write(LuntContext context, IFile target, TTarget value);

        /// <summary>
        /// Gets the type handled by the writer.
        /// </summary>
        /// <returns></returns>
        public Type GetTargetType()
        {
            return typeof (TTarget);
        }

        void ILuntWriter.Write(LuntContext context, IFile target, object value)
        {
            this.Write(context, target, (TTarget) value);
        }
    }
}