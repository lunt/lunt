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
using System.Reflection;
using Lunt.Diagnostics;

namespace Lunt.Runtime
{
    using Lunt.Diagnostics;

    internal sealed class AssemblyScanner
    {
        private readonly IBuildLog _log;

        public AssemblyScanner(IBuildLog log)
        {
            _log = log;
        }

        public IEnumerable<T> Scan<T>(Assembly assembly, bool log = false)
        {
            var types = this.GetLoadableTypes(assembly);
            foreach (Type type in types)
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (typeof (T).IsAssignableFrom(type))
                    {
                        if (log)
                        {
                            _log.Verbose("Found component '{0}'", type.Name);
                        }

                        // Got an empty constructor?
                        var emptyConstructor = type.GetConstructor(Type.EmptyTypes);
                        if (emptyConstructor == null)
                        {
                            _log.Warning(Verbosity.Quiet, "Skipping component '{0}' (no parameterless constructor).", type.FullName);
                            continue;
                        }

                        yield return (T) Activator.CreateInstance(type);
                    }
                }
            }
        }

        private IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}