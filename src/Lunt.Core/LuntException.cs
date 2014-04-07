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
using System.Runtime.Serialization;

namespace Lunt
{
    /// <summary>
    /// Represents errors that occur during execution.
    /// </summary>
    [Serializable]
    public sealed class LuntException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LuntException" /> class.
        /// </summary>
        public LuntException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LuntException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LuntException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public LuntException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="LuntException" /> class from being created.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        private LuntException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}