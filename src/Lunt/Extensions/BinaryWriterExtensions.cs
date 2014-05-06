using System.IO;

namespace Lunt.IO
{
    internal static class BinaryWriterExtensions
    {
        public static void WriteString(this BinaryWriter writer, string value)
        {
            writer.Write(value ?? string.Empty);
        }
    }
}