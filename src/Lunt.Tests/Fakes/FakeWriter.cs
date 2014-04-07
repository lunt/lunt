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
using Moq;

namespace Lunt.Tests.Fakes
{
    public class FakeWriter : ILuntWriter
    {
        private readonly Action<LuntContext, IFile, object> _callback;
        private readonly Type _targetType;

        public FakeWriter(Action<LuntContext, IFile, object> callback, Type targetType)
        {
            _callback = callback;
            _targetType = targetType;
        }


        public Type GetTargetType()
        {
            return _targetType;
        }

        public void Write(LuntContext context, IFile target, object value)
        {
            _callback(context, target, value);
        }
    }

    public class FakeWriter<T> : LuntWriter<T>
    {
        private readonly Action<LuntContext, IFile, T> _action;

        public FakeWriter(Action<LuntContext, IFile, T> action)
        {
            _action = action;
        }

        public override void Write(LuntContext context, IFile target, T value)
        {
            _action(context, target, value);
        }

        public static ILuntWriter Mock(Action<LuntContext, IFile, T> action)
        {
            var mock = new Mock<ILuntWriter>();
            mock.Setup(w => w.GetTargetType()).Returns(typeof (T));
            mock.Setup(w => w.Write(It.IsAny<LuntContext>(), It.IsAny<IFile>(), It.IsAny<object>()))
                .Callback((LuntContext context, IFile file, object obj) => action(context, file, (T) obj));
            return mock.Object;
        }

        public static ILuntWriter MockWithoutCallback(Type targetType)
        {
            var mock = new Mock<ILuntWriter>();
            mock.Setup(w => w.GetTargetType()).Returns(targetType ?? typeof (T));
            mock.Setup(w => w.Write(It.IsAny<LuntContext>(), It.IsAny<IFile>(), It.IsAny<object>()))
                .Callback((LuntContext context, IFile file, object obj) => { });
            return mock.Object;
        }
    }
}