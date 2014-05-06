using System.ComponentModel;
using System.IO;
using Lunt;
using Lunt.IO;

namespace Lake.Tests.Integration.Pipeline
{
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
