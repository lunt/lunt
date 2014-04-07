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
using Lunt.Diagnostics;
using Lunt.IO;

namespace Lunt
{
    using Lunt.Diagnostics;
    using Lunt.IO;

    public sealed class LuntOptions
    {
        public FilePath BuildConfiguration { get; set; }

        public DirectoryPath InputDirectory { get; set; }
        public DirectoryPath OutputDirectory { get; set; }
        public DirectoryPath ProbingDirectory { get; set; }

        public Verbosity Verbosity { get; set; }

        public bool ShowHelp { get; set; }
        public bool ShowVersion { get; set; }

        public bool Rebuild { get; set; }

        public LuntOptions()
        {
            this.Verbosity = Verbosity.Normal;
        }
    }
}