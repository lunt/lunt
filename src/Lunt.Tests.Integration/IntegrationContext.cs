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
using System.IO;
using System.Reflection;

using IODirectory = System.IO.Directory;
using IOFile = System.IO.File;
using IOPath = System.IO.Path;

namespace Lunt.Tests.Integration
{
    public class IntegrationContext : IDisposable
    {
        private readonly string _remotePath;
        private readonly IntegrationAssertion _assert;
        private bool _disposed;

        public string AssetsPath
        {
            get
            {
                return IOPath.Combine(_remotePath, "Assets");
            }
        }

        public string OutputPath
        {
            get
            {
                return IOPath.Combine(_remotePath, "Output");
            }
        }

        public IntegrationAssertion Assert
        {
            get { return _assert; }
        }

        public IntegrationContext()
        {
            // Get the remote path.
            _remotePath = this.GetTemporaryDirectory();
            _assert = new IntegrationAssertion(this);

            // Create the remote directory.
            IODirectory.CreateDirectory(_remotePath);

            // Copy files to the temporary directory.
            this.CopyFiles(_remotePath);
        }

        ~IntegrationContext()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (IODirectory.Exists(_remotePath))
                {
                    IODirectory.Delete(_remotePath, true);
                }
                _disposed = true;
            }
        }

        private void CopyFiles(string destination)
        {
            var source = this.GetDataDirectoryPath();
            var files = IODirectory.GetFiles(source, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = file.Substring(source.Length + 1);
                var remotePath = IOPath.Combine(destination, relativePath);

                // Copy the file.
                var remoteFilePath = IOPath.GetDirectoryName(remotePath);
                if (remoteFilePath != null)
                {
                    if (!IODirectory.Exists(remoteFilePath))
                    {
                        IODirectory.CreateDirectory(remoteFilePath);
                    }
                    IOFile.Copy(file, remotePath);
                }
            }
        }

        private string GetDataDirectoryPath()
        {
            var baseDirectory = IOPath.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (baseDirectory == null)
            {
                throw new InvalidOperationException("Could not locate base directory.");
            }
            var dataDirectory = IOPath.Combine(baseDirectory, "Data");
            if (!IODirectory.Exists(dataDirectory))
            {
                throw new InvalidOperationException("Data directory do not exist.");
            }
            return dataDirectory;
        }

        private string GetTemporaryDirectory()
        {
            var directory = IOPath.GetTempFileName();
            directory = directory.Replace(".tmp", string.Empty);
            directory += "-Lunt";
            if (!IODirectory.Exists(_remotePath))
            {
                return directory;
            }
            throw new InvalidOperationException("Could not generate a non-existing temporary directory.");
        }

        public string GetTargetPath(string relativePath)
        {
            return IOPath.Combine(_remotePath, relativePath);
        }
    }
}
