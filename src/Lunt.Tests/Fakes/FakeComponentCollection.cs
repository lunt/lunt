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

namespace Lunt.Tests.Fakes
{
    public class FakeComponentCollection : IPipelineComponentCollection
    {
        private readonly List<ILuntImporter> _importers;
        private readonly List<ILuntProcessor> _processors;
        private readonly List<ILuntWriter> _writers;

        public FakeComponentCollection()
        {
            _importers = new List<ILuntImporter>();
            _processors = new List<ILuntProcessor>();
            _writers = new List<ILuntWriter>();
        }

        public List<ILuntImporter> Importers
        {
            get { return _importers; }
        }

        public List<ILuntProcessor> Processors
        {
            get { return _processors; }
        }

        public List<ILuntWriter> Writers
        {
            get { return _writers; }
        }

        public IEnumerator<IPipelineComponent> GetEnumerator()
        {
            var result = new List<IPipelineComponent>();
            result.AddRange(_importers);
            result.AddRange(_processors);
            result.AddRange(_writers);
            return result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}