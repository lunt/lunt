using System;
using System.Linq;

namespace Lunt.Descriptors
{
    using System.Collections.Generic;

    /// <summary>
    /// The component registry contains all components that are used for building an asset.
    /// <remarks>
    /// The component registry's only responsibility is to find the components.
    /// Since it lacks real context of what's being built, type checking and such things
    /// must be the responsibility of the caller when retrieving a component.
    /// </remarks>
    /// </summary>
    internal sealed class DescriptorRegistry
    {
        private readonly ImporterDescriptor[] _importers;
        private readonly ProcessorDescriptor[] _processors;
        private readonly WriterDescriptor[] _writers;

        public DescriptorRegistry(IEnumerable<IPipelineComponent> components)
        {
            var pipelineComponents = components as IPipelineComponent[] ?? components.ToArray();
            _importers = DescriptorFactory.CreateImporterDescriptors(pipelineComponents);
            _processors = DescriptorFactory.CreateProcessorDescriptors(pipelineComponents);
            _writers = DescriptorFactory.CreateWriterDescriptors(pipelineComponents);
        }

        public ImporterDescriptor GetImporter(Asset asset)
        {
            // Get the extension.
            var extension = asset.Path.GetExtension();

            // Find the importer.
            return _importers.FirstOrDefault(i => i.Extensions.Contains(extension, StringComparer.OrdinalIgnoreCase));
        }

        public ProcessorDescriptor GetProcessor(Asset asset)
        {
            // Got a processor specified?
            if (!string.IsNullOrWhiteSpace(asset.ProcessorName))
            {
                var processorInfo = _processors.FirstOrDefault(p => p.Name.Equals(asset.ProcessorName, StringComparison.OrdinalIgnoreCase));
                if (processorInfo != null)
                {
                    return processorInfo;
                }
                return null;
            }

            // Find the importer for the asset.
            var importerInfo = GetImporter(asset);
            if (importerInfo != null)
            {
                // Got a default processor?				
                if (importerInfo.DefaultProcessor != null)
                {
                    var processorInfo = _processors.FirstOrDefault(p => p.Processor.GetType() == importerInfo.DefaultProcessor);
                    if (processorInfo != null)
                    {
                        return processorInfo;
                    }
                }
            }

            return null;
        }

        public WriterDescriptor GetWriter(Type type)
        {
            var writerInfo = _writers.FirstOrDefault(w => w.TargetType == type);
            if (writerInfo != null)
            {
                return writerInfo;
            }

            return null;
        }
    }
}