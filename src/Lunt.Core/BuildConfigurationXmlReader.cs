// ﻿
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
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Lunt.IO;

namespace Lunt
{
    using Lunt.IO;

    /// <summary>
    /// Provides a mechanism for reading build configurations from XML.
    /// </summary>
    public sealed class BuildConfigurationXmlReader : IBuildConfigurationReader
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildConfigurationXmlReader"/> class.
        /// </summary>
        /// <param name="fileSystem"></param>
        public BuildConfigurationXmlReader(IFileSystem fileSystem)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Reads a build configuration from an XML file in the file system.
        /// </summary>
        /// <param name="path">The build configuration file path.</param>
        /// <returns>A build configuration read from the specified XML file.</returns>
        public BuildConfiguration Read(FilePath path)
        {
            // Get a representation of the configuration file.
            var configurationFile = _fileSystem.GetFile(path);
            if (!configurationFile.Exists)
            {
                throw new LuntException("Build configuration file do not exist.");
            }

            // Read the build configuration.
            using (var stream = configurationFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                try
                {
                    var document = XDocument.Load(stream);
                    return Read(document);
                }
                catch (Exception ex)
                {
                    throw new LuntException("Could not load build configuration.", ex);
                }
            }
        }

        /// <summary>
        /// Reads a build configuration from an XML file.
        /// </summary>
        /// <returns>A build configuration read from the specified XML file.</returns>
        public BuildConfiguration Read(XDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            var configuration = new BuildConfiguration();

            // Make sure the root element is correct.
            XElement buildElement = HasRoot(document, "build");
            if (buildElement == null)
            {
                throw new LuntException("Configuration XML is missing 'build' element.");
            }

            // Parse the content.
            foreach (XElement contentElement in GetElements(document.Root, "asset", StringComparison.OrdinalIgnoreCase))
            {
                // Get the path.
                string path = GetAttributeValue(contentElement, "path");
                if (path != null)
                {
                    if (string.IsNullOrWhiteSpace(path))
                    {
                        throw new LuntException("Asset element contains empty 'path' attribute.");
                    }
                }

                // Get the processor (if one defined).
                string processor = GetAttributeValue(contentElement, "processor");
                if (processor != null)
                {
                    processor = processor.Trim();
                    if (string.IsNullOrWhiteSpace(processor))
                    {
                        processor = null;
                    }
                }

                // Parse metadata (if any).
                var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (XElement metadataElement in GetElements(contentElement, "metadata", StringComparison.OrdinalIgnoreCase))
                {
                    // Get the key.
                    string key = GetAttributeValue(metadataElement, "key");
                    if (key == null)
                    {
                        throw new LuntException("Metadata element is missing 'key' attribute.");
                    }
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        throw new LuntException("Metadata element contains empty 'key' attribute.");
                    }

                    // Add metadata to asset.
                    metadata.Add(key, metadataElement.Value);
                }

                // Create the asset definition and add it to the configuration.
                var asset = new AssetDefinition(new FilePath(path), metadata); 
                asset.ProcessorName = processor;
                configuration.Assets.Add(asset);
            }

            return configuration;
        }

        private static XElement HasRoot(XDocument document, string name)
        {
            if (document.Root != null)
            {
                var rootName = document.Root.Name.LocalName;
                if (rootName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return document.Root;
                }
            }
            return null;
        }

        private static IEnumerable<XElement> GetElements(XElement element, string name, StringComparison comparison)
        {
            return element.Elements().Where(e => e.Name.LocalName.Equals(name, comparison));
        }

        private static string GetAttributeValue(XElement element, string attributeName)
        {
            var attributes = element.Attributes();
            var attribute = attributes.FirstOrDefault(x => x.Name.LocalName.Equals(attributeName, StringComparison.OrdinalIgnoreCase));
            return attribute != null ? attribute.Value : null;
        }
    }
}