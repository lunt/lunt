using System;
using Xunit;

namespace Lunt.Testing
{
    public static class ShouldExtensions
    {
        public static ArgumentException ShouldHaveParameter(this ArgumentException exception, string parameterName)
        {
            Assert.Equal(parameterName, exception.ParamName);
            return exception;
        }

        public static T ShouldHaveMessage<T>(this T exception, string message)
            where T : Exception
        {
            Assert.Equal(message, exception.Message);
            return exception;
        }
    }
}
