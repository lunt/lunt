using Lunt.IO;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class LuntImporterTests
    {
        [Fact]
        public void Will_Delegate_Import_Call_From_Explicit_Interface_Member_To_Typed_Implementation()
        {
            // Given
            var result = false;
            var mock = new Mock<LuntImporter<string>>();
            mock.Setup(x => x.Import(It.IsAny<LuntContext>(), It.IsAny<IFile>())).Callback(() => result = true);

            // When
            ((ILuntImporter) mock.Object).Import(null, null);

            // Then
            Assert.True(result);
        }
    }
}