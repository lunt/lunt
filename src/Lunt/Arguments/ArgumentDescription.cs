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
using System.Reflection;

namespace Lunt.Arguments
{
    internal sealed class ArgumentDescription
    {
        private readonly string[] _names;
        private readonly string _parameter;
        private readonly bool _required;
        private readonly string _description;
        private readonly PropertyInfo _action;

        public string[] Names
        {
            get { return _names; }
        }

        public string Parameter
        {
            get { return _parameter; }
        }

        public bool Required
        {
            get { return _required; }
        }

        public string Description
        {
            get { return _description; }
        }

        internal PropertyInfo Property
        {
            get { return _action; }
        }

        public ArgumentDescription(string[] names, string parameter, bool required, string description, PropertyInfo action)
        {
            _names = names;
            _parameter = parameter;
            _required = required;
            _description = description;
            _action = action;
        }
    }
}