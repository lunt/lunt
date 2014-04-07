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

namespace Lunt.Descriptors
{
    internal sealed class ProcessorDescriptor : ComponentDescriptor
    {
        private readonly ILuntProcessor _processor;
        private readonly Type _sourceType;
        private readonly Type _targetType;

        public ILuntProcessor Processor
        {
            get { return _processor; }
        }

        public Type SourceType
        {
            get { return _sourceType; }
        }

        public Type TargetType
        {
            get { return _targetType; }
        }

        public ProcessorDescriptor(ILuntProcessor processor)
        {
            _processor = processor;
            _sourceType = processor.GetSourceType();
            _targetType = processor.GetTargetType();
        }

        public override Type GetComponentType()
        {
            return _processor.GetType();
        }
    }
}