using System;

namespace Lunt.Descriptors
{
    internal sealed class ImporterDescriptor : ComponentDescriptor
    {
        private readonly IImporter _importer;
        private readonly string[] _extensions;
        private readonly Type _defaultProcessor;

        public IImporter Importer
        {
            get { return _importer; }
        }

        public string[] Extensions
        {
            get { return _extensions; }
        }

        public Type DefaultProcessor
        {
            get { return _defaultProcessor; }
        }

        internal ImporterDescriptor(IImporter importer, ImporterAttribute attribute)
        {
            _importer = importer;
            _extensions = attribute.FileExtensions;
            _defaultProcessor = attribute.DefaultProcessor;
        }

        public override Type GetComponentType()
        {
            return _importer.GetType();
        }
    }
}