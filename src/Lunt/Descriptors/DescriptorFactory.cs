using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Lunt.Descriptors
{
    internal static class DescriptorFactory
    {
        public static ImporterDescriptor[] CreateImporterDescriptors(IEnumerable<IPipelineComponent> components)
        {
            var importers = new HashSet<ImporterDescriptor>();
            var importerTypes = new List<Type>();
            var registeredExtensions = new HashSet<string>();

            foreach (var importer in components.OfType<IImporter>())
            {
                var importerType = importer.GetType();

                var attribute = importerType.GetAttribute<ImporterAttribute>();
                if (attribute == null)
                {
                    const string format = "The importer {0} has not been decorated with an importer attribute.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, importerType.FullName);
                    throw new LuntException(message);
                }

                // No extensions?
                if (attribute.FileExtensions.Length == 0 || attribute.FileExtensions[0] == null)
                {
                    const string format = "The importer {0} has not been associated with any file extensions.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, importerType.FullName);
                    throw new LuntException(message);
                }

                // Iterate through all extensions.
                foreach (var extension in attribute.FileExtensions.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    // Extension already registered?
                    if (registeredExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        const string format = "More than one importer has been associated with the file extension '{0}'.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, extension);
                        throw new LuntException(message);
                    }

                    // Add extensions to registered extensions.
                    registeredExtensions.Add(extension);
                }

                // Got a default processor?
                if (attribute.DefaultProcessor != null)
                {
                    if (!typeof (IProcessor).IsAssignableFrom(attribute.DefaultProcessor))
                    {
                        const string format = "The default processor ({0}) referenced by {1} is not a processor.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, attribute.DefaultProcessor.FullName, importerType.FullName);
                        throw new LuntException(message);
                    }

                    if (attribute.DefaultProcessor.IsAbstract)
                    {
                        const string format = "The default processor ({0}) referenced by {1} is abstract.";
                        var message = string.Format(CultureInfo.InvariantCulture, format, attribute.DefaultProcessor.FullName, importerType.FullName);
                        throw new LuntException(message);
                    }
                }

                // Create the descriptor.
                var descriptor = new ImporterDescriptor(importer, attribute);
                GetName(descriptor);

                // Add the descriptor to the result.
                importers.Add(descriptor);
                importerTypes.Add(importerType);
            }
            return importers.ToArray();
        }

        public static ProcessorDescriptor[] CreateProcessorDescriptors(IEnumerable<IPipelineComponent> components)
        {
            var processors = new List<ProcessorDescriptor>();
            var processorTypes = new HashSet<Type>();

            foreach (var processor in components.OfType<IProcessor>())
            {
                var processorType = processor.GetType();

                if (processor.GetSourceType() == null)
                {
                    const string format = "The processor {0} has no source type.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, processorType.FullName);
                    throw new LuntException(message);
                }
                if (processor.GetTargetType() == null)
                {
                    const string format = "The processor {0} has no target type.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, processorType.FullName);
                    throw new LuntException(message);
                }

                // Create the descriptor.
                var descriptor = new ProcessorDescriptor(processor);
                GetName(descriptor);

                processors.Add(descriptor);
                processorTypes.Add(processorType);
            }

            return processors.ToArray();
        }

        public static WriterDescriptor[] CreateWriterDescriptors(IEnumerable<IPipelineComponent> components)
        {
            var writers = new List<WriterDescriptor>();
            var writerTypes = new HashSet<Type>();

            foreach (var writer in components.OfType<IWriter>())
            {
                var writerType = writer.GetType();

                var targetType = writer.GetTargetType();
                if (writer.GetTargetType() == null)
                {
                    const string format = "The writer {0} has no target type.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, writerType.FullName);
                    throw new LuntException(message);
                }

                if (writers.Select(w => w.TargetType).Any(registeredTargetType => registeredTargetType == targetType))
                {
                    const string format = "More than one writer has been associated with the type {0}.";
                    var message = string.Format(CultureInfo.InvariantCulture, format, targetType.FullName);
                    throw new LuntException(message);
                }

                // Create the descriptor.
                var descriptor = new WriterDescriptor(writer);
                GetName(descriptor);

                writers.Add(descriptor);
                writerTypes.Add(writerType);
            }

            return writers.ToArray();
        }

        private static void GetName(ComponentDescriptor descriptor)
        {
            var descriptorType = descriptor.GetComponentType();
            var name = descriptorType.Name;

            var nameAttribute = descriptorType.GetAttribute<DisplayNameAttribute>();
            if (nameAttribute != null)
            {
                name = nameAttribute.DisplayName;
            }

            descriptor.Name = name;
        }
    }
}