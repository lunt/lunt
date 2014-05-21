using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lunt.IO;
using NSubstitute;
using Xunit;

namespace Lunt.Tests.Unit
{
    public class ProcessorTests
    {
        [Fact]
        public void Will_Delegate_Import_Call_From_Explicit_Interface_Member_To_Typed_Implementation()
        {
            // Given
            var processor = Substitute.For<Processor<string, string>>();

            // When
            ((IProcessor)processor).Process(null, null);

            // Then
            processor.Received(1).Process(Arg.Any<Context>(), Arg.Any<string>());
        }
    }
}
