using System;
using Lunt.IO;
using Lunt.Testing;
using Xunit;

namespace Lunt.Tests.Unit.IO
{
    public class HashComputerTests
    {
        [Fact]
        public void Disposed_Instance_Should_Throw_If_Asked_To_Compute_Hash()
        {
            // Given
            var hasher = new HashComputer();
            var filesystem = new FileSystem();
            var file = filesystem.GetFile("file.data").Create("Hello World");
            hasher.Dispose();

            // When
            var result = Record.Exception(() => hasher.Compute(file));

            // Then
            Assert.IsType<ObjectDisposedException>(result);
            Assert.Equal("Lunt.IO.HashComputer", ((ObjectDisposedException) result).ObjectName);
        }

        [Fact]
        public void Same_Input_Returns_The_Same_Hashes()
        {
            // Given
            var hasher = new HashComputer();
            var filesystem = new FileSystem();
            var file = filesystem.GetFile("file.data").Create("Hello World");

            // When
            string first = hasher.Compute(file);
            string second = hasher.Compute(file);

            // Then
            Assert.Equal(first, second);
        }

        [Fact]
        public void Different_Input_Returns_Different_Hashes()
        {
            // Given
            var hasher = new HashComputer();
            var filesystem = new FileSystem();
            var file1 = filesystem.GetFile("file1.data").Create("Hello World");
            var file2 = filesystem.GetFile("file2.data").Create("Goodbye World");

            // When
            string first = hasher.Compute(file1);
            string second = hasher.Compute(file2);

            // Then
            Assert.NotEqual(first, second);
        }
    }
}