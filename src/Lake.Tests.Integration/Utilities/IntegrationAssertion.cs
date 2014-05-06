using System.IO;
using Xunit;
using Xunit.Sdk;

namespace Lake.Tests.Integration
{
    public class IntegrationAssertion
    {
        private readonly IntegrationContext _context;

        public IntegrationAssertion(IntegrationContext context)
        {
            _context = context;
        }

        public void ApplicationExitedWithoutError(IntegrationTestResult result)
        {
            if (result == null || result.ExitCode != 0)
            {
                throw new AssertException("The application exited with an error.");
            }
        }

        public void TargetFileExists(string filename)
        {
            filename = _context.GetTargetPath(filename);
            if (!File.Exists(filename))
            {
                const string format = "The file '{0}' did not exist.";
                string message = string.Format(format, filename);
                throw new AssertException(message);
            }
        }

        public void HasContent(string filename, string content)
        {
            TargetFileExists(filename);
            filename = _context.GetTargetPath(filename);
            var readContent = File.ReadAllText(filename);
            Assert.Equal(content, readContent);
        }
    }
}
