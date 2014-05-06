using System.Collections;
using System.Collections.Generic;

namespace Lunt.Testing
{
    public class FakeComponentCollection : IPipelineComponentCollection
    {
        private readonly List<ILuntImporter> _importers;
        private readonly List<ILuntProcessor> _processors;
        private readonly List<ILuntWriter> _writers;

        public FakeComponentCollection()
        {
            _importers = new List<ILuntImporter>();
            _processors = new List<ILuntProcessor>();
            _writers = new List<ILuntWriter>();
        }

        public List<ILuntImporter> Importers
        {
            get { return _importers; }
        }

        public List<ILuntProcessor> Processors
        {
            get { return _processors; }
        }

        public List<ILuntWriter> Writers
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