using Lunt.IO;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class LuntWriterTests
    {
        [Fact]
        public void Will_Delegate_Import_Call_From_Explicit_Interface_Member_To_Typed_Implementation()
        {
            // Given
            var result = false;
            var mock = new Mock<LuntWriter<string>>();
            mock.Setup(x => x.Write(It.IsAny<LuntContext>(), It.IsAny<IFile>(), It.IsAny<string>())).Callback(() => result = true);

            // When
            ((ILuntWriter) mock.Object).Write(null, null, null);

            // Then
            Assert.True(result);
        }
    }
}