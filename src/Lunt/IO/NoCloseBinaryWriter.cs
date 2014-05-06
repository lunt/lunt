using System.IO;

namespace Lunt.IO
{
    internal sealed class NoCloseBinaryWriter : BinaryWriter
    {
        public NoCloseBinaryWriter(Stream stream)
            : base(stream)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(false);
        }
    }
}