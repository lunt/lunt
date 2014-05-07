using System.Collections;
using System.Collections.Generic;

namespace Lunt.Testing
{
    public class FakeComponentCollection : IPipelineComponentCollection
    {
        private readonly List<IImporter> _importers;
        private readonly List<IProcessor> _processors;
        private readonly List<IWriter> _writers;

        public FakeComponentCollection()
        {
            _importers = new List<IImporter>();
            _processors = new List<IProcessor>();
            _writers = new List<IWriter>();
        }

        public List<IImporter> Importers
        {
            get { return _importers; }
        }

        public List<IProcessor> Processors
        {
            get { return _processors; }
        }

        public List<IWriter> Writers
        {
            get { return _writers; }
        }

        public IEnumerator<IPipelineComponent> GetEnumerator()
        {
            var result = new List<IPipelineComponent>();
            result.AddRange(_importers);
            result.AddRange(_processors);
            result.AddRange(_writers);
            return result.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}