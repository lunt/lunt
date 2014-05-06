using System.IO;
using System.Text;
using Lunt.IO;

namespace Lunt.Testing
{
    public static class FileExtensions
    {
        public static Stream Create(this IFile file)
        {
            return file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        }

        public static IFile Create(this IFile file, string data)
        {
            return Create(file, Encoding.UTF8.GetBytes(data));
        }

        public static IFile Create(this IFile file, byte[] data)
        {
            if (file != null)
            {
                using (var stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return file;
        }
    }
}