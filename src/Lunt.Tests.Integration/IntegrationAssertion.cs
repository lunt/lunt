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
using System.IO;
using Xunit;
using Xunit.Sdk;

namespace Lunt.Tests.Integration
{
    public class IntegrationAssertion
    {
        private readonly IntegrationContext _context;

        public IntegrationAssertion(IntegrationContext context)
        {
            _context = context;
        }

        public void ApplicationExitedWithoutError(IntegrationTestResult result)
        {
            if (result == null || result.ExitCode != 0)
            {
                throw new AssertException("The application exited with an error.");
            }
        }

        public void TargetFileExists(string filename)
        {
            filename = _context.GetTargetPath(filename);
            if (!File.Exists(filename))
            {
                const string format = "The file '{0}' did not exist.";
                string message = string.Format(format, filename);
                throw new AssertException(message);
            }
        }

        public void HasContent(string filename, string content)
        {
            this.TargetFileExists(filename);
            filename = _context.GetTargetPath(filename);
            var readContent = File.ReadAllText(filename);
            Assert.Equal(content, readContent);
        }
    }
}
