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
    /// A console output writer that writes to the standard output stream.
    /// </summary>
    public sealed class ConsoleWriter : IConsoleWriter
    {
        /// <summary>
        /// Writes the text representation of the specified array of objects, 
        /// followed by the current line terminator to the standard output stream.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void WriteLine(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        /// <summary>
        /// Sets the forground console color.
        /// </summary>
        /// <param name="color">The foreground color to set.</param>
        public void SetForeground(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Sets the foreground console color to it's default.
        /// </summary>
        public void ResetColors()
        {
            Console.ResetColor();
        }
    }
}