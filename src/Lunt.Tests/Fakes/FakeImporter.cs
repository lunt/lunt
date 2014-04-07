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
using Lunt.Tests.Utilities;

namespace Lunt.Tests.Fakes
{
    public class FakeImporter<T> : ILuntImporter
    {
        private readonly Func<LuntContext, IFile, T> _func;

        public FakeImporter(Func<LuntContext, IFile, T> func)
        {
            _func = func;
        }

        public static ILuntImporter Mock(Func<LuntContext, IFile, T> func, string extension, Type defaultProcessor = null)
        {
            return ImporterGenerator.Create(defaultProcessor, typeof (T), func, extension);
        }

        object ILuntImporter.Import(LuntContext context, IFile file)
        {
            return _func(context, file);
        }
    }
}