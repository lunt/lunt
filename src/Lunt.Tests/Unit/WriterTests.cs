using Lunt.IO;
using NSubstitute;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class WriterTests
    {
        [Fact]
        public void Will_Delegate_Import_Call_From_Explicit_Interface_Member_To_Typed_Implementation()
        {
            // Given
            var writer = Substitute.For<Writer<string>>();

            // When
            ((IWriter)writer).Write(null, null, null);

            // Then
            writer.Received(1).Write(Arg.Any<Context>(), Arg.Any<IFile>(), Arg.Any<string>());
        }
    }
}