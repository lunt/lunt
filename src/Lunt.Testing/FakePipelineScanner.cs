using System.Collections.Generic;
using Lunt.Runtime;
using Lunt.Testing;

namespace Lunt.Tests.Framework
{
    public class FakePipelineScanner : IPipelineScanner
    {
        private readonly IPipelineComponentCollection _components;

        public FakePipelineScanner()
        {
            _components = new FakeComponentCollection();
        }

        public FakePipelineScanner(IPipelineComponentCollection components)
        {
            _components = components;
        }

        public IEnumerable<IPipelineComponent> Scan()
        {
            return _components;
        }
    }
}
