using System.ComponentModel;
using System.IO;
using Lunt;
using Lunt.IO;

namespace Lake.Tests.Integration.Pipeline
{
    [DisplayName("Text Writer")]
    public sealed class TextWriter : Writer<string>
    {
        public override void Write(Context context, IFile target, string value)
        {
            using (var stream = target.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(value);
            }
        }
    }
}