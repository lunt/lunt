using Xunit;

namespace Lunt.Tests.Unit
{
    public class LuntImporterAttributeTests
    {
        public class TheConstructor
        {
            [Fact]
            public void Importer_Can_Target_Single_File_Extension()
            {
                // Given, When
                var attribute = new LuntImporterAttribute(".txt");

                // Then
                Assert.Equal(1, attribute.FileExtensions.Length);
            }

            [Fact]
            public void Importer_Can_Target_Multiple_File_Extension()
            {
                // Given, When
                var attribute = new LuntImporterAttribute(".txt", ".png");

                // Then
                Assert.Equal(2, attribute.FileExtensions.Length);
            }

            [Fact]
            public void Default_Processor_Should_Be_Set_To_Null_If_Not_Provided()
            {
                // Given, When
                var attribute = new LuntImporterAttribute(".txt");

                // Then
                Assert.Null(attribute.DefaultProcessor);
            }

            [Fact]
            public void Can_Set_Default_Processor()
            {
                // Given, When
                var attribute = new LuntImporterAttribute(".txt");
                attribute.DefaultProcessor = typeof (string);

                // Then
                Assert.Equal(typeof (string), attribute.DefaultProcessor);
            }
        }
    }
}