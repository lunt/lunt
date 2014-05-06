using System;

namespace Lunt.Descriptors
{
    internal sealed class ImporterDescriptor : ComponentDescriptor
    {
        private readonly ILuntImporter _importer;
        private readonly string[] _extensions;
        private readonly Type _defaultProcessor;

        public ILuntImporter Importer
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

        internal ImporterDescriptor(ILuntImporter importer, LuntImporterAttribute attribute)
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