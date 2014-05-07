using System;
using System.IO;
using System.Reflection;

namespace Lake.Tests.Integration
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
                return Path.Combine(_remotePath, "Assets");
            }
        }

        public string OutputPath
        {
            get
            {
                return Path.Combine(_remotePath, "Output");
            }
        }

        public IntegrationAssertion Assert
        {
            get { return _assert; }
        }

        public IntegrationContext()
        {
            // Get the remote path.
            _remotePath = GetTemporaryDirectory();
            _assert = new IntegrationAssertion(this);

            // Create the remote directory.
            Directory.CreateDirectory(_remotePath);

            // Copy files to the temporary directory.
            CopyFiles(_remotePath);
        }

        ~IntegrationContext()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);            
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (Directory.Exists(_remotePath))
                {
                    Directory.Delete(_remotePath, true);
                }
                _disposed = true;
            }
        }

        private static void CopyFiles(string destination)
        {
            var source = GetDataDirectoryPath();
            var files = Directory.GetFiles(source, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = file.Substring(source.Length + 1);
                var remotePath = Path.Combine(destination, relativePath);

                // Copy the file.
                var remoteFilePath = Path.GetDirectoryName(remotePath);
                if (remoteFilePath != null)
                {
                    if (!Directory.Exists(remoteFilePath))
                    {
                        Directory.CreateDirectory(remoteFilePath);
                    }
                    File.Copy(file, remotePath);
                }
            }
        }

        private static string GetDataDirectoryPath()
        {
            var baseDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            if (baseDirectory == null)
            {
                throw new InvalidOperationException("Could not locate base directory.");
            }
            var dataDirectory = Path.Combine(baseDirectory, "Data");
            if (!Directory.Exists(dataDirectory))
            {
                throw new InvalidOperationException("Data directory do not exist.");
            }
            return dataDirectory;
        }

        private string GetTemporaryDirectory()
        {
            var directory = Path.GetTempFileName();
            directory = directory.Replace(".tmp", string.Empty);
            directory += "-Lunt";
            if (!Directory.Exists(_remotePath))
            {
                return directory;
            }
            throw new InvalidOperationException("Could not generate a non-existing temporary directory.");
        }

        public string GetTargetPath(string relativePath)
        {
            return Path.Combine(_remotePath, relativePath);
        }
    }
}
