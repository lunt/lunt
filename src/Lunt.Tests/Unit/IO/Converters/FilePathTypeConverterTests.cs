﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using Lunt.IO;
using Xunit;

namespace Lunt.Tests.Unit.IO.Converters
{
    public class FilePathTypeConverterTests
    {
        [Fact]
        public void Can_Get_Converter()
        {
            // Given, When
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // Then
            Assert.NotNull(converter);
            Assert.Equal("Lunt.IO.FilePathTypeConverter", converter.GetType().FullName);
        }

        [Fact]
        public void Can_Convert_From_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = converter.CanConvertFrom(typeof (string));

            // Then
            Assert.True(result);
        }

        [Fact]
        public void Can_Perform_Convertion_From_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = converter.ConvertFromString("/hello/world") as FilePath;

            // Then
            Assert.NotNull(result);
            Assert.Equal("/hello/world", result.FullPath);
        }

        [Fact]
        public void Should_Not_Indicate_That_Non_Strings_Can_Be_Converted()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = converter.CanConvertFrom(typeof (int));

            // Then
            Assert.False(result);
        }

        [Fact]
        public void Should_Throw_When_Converting_From_Non_String()
        {
            // Given
            var converter = TypeDescriptor.GetConverter(typeof (FilePath));

            // When
            var result = Record.Exception(() => converter.ConvertFrom(new List<string>()));

            // Then
            Assert.IsType<NotSupportedException>(result);
        }
    }
}