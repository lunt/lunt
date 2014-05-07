using Lunt.IO;
using Moq;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class ImporterTests
    {
        [Fact]
        public void Will_Delegate_Import_Call_From_Explicit_Interface_Member_To_Typed_Implementation()
        {
            // Given
            var result = false;
            var mock = new Mock<Importer<string>>();
            mock.Setup(x => x.Import(It.IsAny<Context>(), It.IsAny<IFile>())).Callback(() => result = true);

            // When
            ((IImporter) mock.Object).Import(null, null);

            // Then
            Assert.True(result);
        }
    }
}