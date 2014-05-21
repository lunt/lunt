using Lunt.IO;
using NSubstitute;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class ImporterTests
    {
        [Fact]
        public void Will_Delegate_Import_Call_From_Explicit_Interface_Member_To_Typed_Implementation()
        {
            // Given
            var writer = Substitute.For<Importer<string>>();

            // When
            ((IImporter)writer).Import(null, null);

            // Then
            writer.Received(1).Import(Arg.Any<Context>(), Arg.Any<IFile>());
        }
    }
}