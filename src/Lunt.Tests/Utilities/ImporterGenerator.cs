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
using System.Diagnostics;
using System.Reflection.Emit;
using Castle.DynamicProxy;
using Lunt.IO;
using Lunt.Tests.Fakes;

namespace Lunt.Tests.Utilities
{
    public class ImporterGenerator
    {
        public static ILuntImporter Create<T>(Type defaultProcessor, Type sourceType, Func<LuntContext, IFile, T> func, params string[] extensions)
        {
            // Get attribute builder.
            Type[] ctorTypes = {typeof (string[]), typeof (Type)};
            var ctor = typeof (LuntImporterAttribute).GetConstructor(ctorTypes);
            Debug.Assert(ctor != null, "Could not get constructor for content importer attribute.");
            object[] arguments = {extensions, defaultProcessor};
            var builder = new CustomAttributeBuilder(ctor, arguments);

            // Create the procy generatin options.
            var proxyOptions = new ProxyGenerationOptions();
            proxyOptions.AdditionalAttributes.Add(builder);

            // Create the proxy generator and create the proxy.
            var proxyGenerator = new ProxyGenerator();
            return (ILuntImporter) proxyGenerator.CreateClassProxy(typeof (FakeImporter<T>), proxyOptions, new object[] {func});
        }
    }
}