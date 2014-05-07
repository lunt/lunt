using Lunt.IO;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class WriterTests
    {
        [Fact]
        public void Will_Delegate_Import_Call_From_Explicit_Interface_Member_To_Typed_Implementation()
        {
            // Given
            var result = false;
            var mock = new Mock<Writer<string>>();
            mock.Setup(x => x.Write(It.IsAny<Context>(), It.IsAny<IFile>(), It.IsAny<string>())).Callback(() => result = true);

            // When
            ((IWriter) mock.Object).Write(null, null, null);

            // Then
            Assert.True(result);
        }
    }
}