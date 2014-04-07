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
using System.ComponentModel;
using System.IO;
using Lunt.IO;

namespace Lunt.Tests.Integration.Pipeline
{
    using Lunt.IO;

    [DisplayName("Text Importer")]
    [LuntImporter(".txt")]
    public class TextImporter : LuntImporter<string>
    {
        public override string Import(LuntContext context, IFile source)
        {
            using (var stream = source.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(stream))
            {
                // Add a dependency to metadata.
                var metadataFile = context.FileSystem.GetFile(source.Path.FullPath + ".metadata");
                if (metadataFile.Exists)
                {
                    context.AddDependency(metadataFile);
                }

                return reader.ReadToEnd();
            }
        }
    }
}
