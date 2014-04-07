﻿// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of Lunt.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Lunt.Descriptors
{
    internal static class DescriptorFactory
    {
        public static ImporterDescriptor[] CreateImporterDescriptors(IPipelineComponentCollection components)
        {
            var importers = new HashSet<ImporterDescriptor>();
            var importerTypes = new List<Type>();
            var registeredExtensions = new HashSet<string>();

            foreach (var importer in components.OfType<ILuntImporter>())
            {
                var importerType = importer.GetType();

                var attribute = importerType.GetAttribute<LuntImporterAttribute>();
                if (attribute == null)
                {
                    const string format = "The importer {0} has not been decorated with an importer attribute.";
                    string message = string.Format(CultureInfo.InvariantCulture, format, importerType.FullName);
                    throw new LuntException(message);
                }

                // No extensions?
                if (attribute.FileExtensions.Length == 0 || attribute.FileExtensions[0] == null)
                {
                    const string format = "The importer {0} has not been associated with any file extensions.";
                    string message = string.Format(CultureInfo.InvariantCulture, format, importerType.FullName);
                    throw new LuntException(message);
                }

                // Iterate through all extensions.
                foreach (var extension in attribute.FileExtensions.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    // Extension already registered?
                    if (registeredExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase))
                    {
                        const string format = "More than one importer has been associated with the file extension '{0}'.";
                        string message = string.Format(CultureInfo.InvariantCulture, format, extension);
                        throw new LuntException(message);
                    }

                    // Add extensions to registered extensions.
                    registeredExtensions.Add(extension);
                }

                // Got a default processor?
                if (attribute.DefaultProcessor != null)
                {
                    if (!typeof (ILuntProcessor).IsAssignableFrom(attribute.DefaultProcessor))
                    {
                        const string format = "The default processor ({0}) referenced by {1} is not a processor.";
                        string message = string.Format(CultureInfo.InvariantCulture, format, attribute.DefaultProcessor.FullName, importerType.FullName);
                        throw new LuntException(message);
                    }

                    if (attribute.DefaultProcessor.IsAbstract)
                    {
                        const string format = "The default processor ({0}) referenced by {1} is abstract.";
                        string message = string.Format(CultureInfo.InvariantCulture, format, attribute.DefaultProcessor.FullName, importerType.FullName);
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

        public static ProcessorDescriptor[] CreateProcessorDescriptors(IPipelineComponentCollection components)
        {
            var processors = new List<ProcessorDescriptor>();
            var processorTypes = new HashSet<Type>();

            foreach (var processor in components.OfType<ILuntProcessor>())
            {
                var processorType = processor.GetType();

                if (processor.GetSourceType() == null)
                {
                    const string format = "The processor {0} has no source type.";
                    string message = string.Format(CultureInfo.InvariantCulture, format, processorType.FullName);
                    throw new LuntException(message);
                }
                if (processor.GetTargetType() == null)
                {
                    const string format = "The processor {0} has no target type.";
                    string message = string.Format(CultureInfo.InvariantCulture, format, processorType.FullName);
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

        public static WriterDescriptor[] CreateWriterDescriptors(IPipelineComponentCollection components)
        {
            var writers = new List<WriterDescriptor>();
            var writerTypes = new HashSet<Type>();

            foreach (var writer in components.OfType<ILuntWriter>())
            {
                var writerType = writer.GetType();

                var targetType = writer.GetTargetType();
                if (writer.GetTargetType() == null)
                {
                    const string format = "The writer {0} has no target type.";
                    string message = string.Format(CultureInfo.InvariantCulture, format, writerType.FullName);
                    throw new LuntException(message);
                }

                if (writers.Select(w => w.TargetType).Any(registeredTargetType => registeredTargetType == targetType))
                {
                    const string format = "More than one writer has been associated with the type {0}.";
                    string message = string.Format(CultureInfo.InvariantCulture, format, targetType.FullName);
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
            Type descriptorType = descriptor.GetComponentType();
            string name = descriptorType.Name;

            var nameAttribute = descriptorType.GetAttribute<DisplayNameAttribute>();
            if (nameAttribute != null)
            {
                name = nameAttribute.DisplayName;
            }

            descriptor.Name = name;
        }
    }
}