using System.IO;

namespace Lunt.IO
{
    internal sealed class NoCloseBinaryReader : BinaryReader
    {
        public NoCloseBinaryReader(Stream stream)
            : base(stream)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(false);
        }
    }
}