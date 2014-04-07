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
using Lunt.IO;

namespace Lunt
{
    using Lunt.IO;

    internal sealed class AssetExtractor
    {
        private readonly IBuildEnvironment _environment;
        private readonly Globber _globber;

        public AssetExtractor(IBuildEnvironment environment)
        {
            _environment = environment;
            _globber = new Globber(environment);
        }

        public Asset[] Extract(BuildConfiguration configuration)
        {
            var result = new List<Asset>();
            foreach (var definition in configuration.Assets)
            {
                if (definition.IsGlob)
                {
                    // Path is a glob pattern.
                    result.AddRange(this.ExtractGlob(configuration, definition));
                }
                else
                {
                    // Path is a single asset.
                    var path = new FilePath(definition.Path.FullPath);
                    result.Add(this.CreateAsset(path, definition));
                }
            }
            return result.ToArray();
        }

        private IEnumerable<Asset> ExtractGlob(BuildConfiguration configuration, AssetDefinition definition)
        {
            var pattern = this.GetGlobPattern(configuration, definition);
            var result = _globber.Glob(pattern);
            foreach (var item in result)
            {
                var inputDirectoryLength = configuration.InputDirectory.FullPath.Length + 1;
                var path = new FilePath(item.FullPath.Substring(inputDirectoryLength));
                yield return this.CreateAsset(path, definition);
            }
        }

        private Asset CreateAsset(FilePath path, AssetDefinition definition)
        {
            var asset = new Asset(path, definition.Metadata);
            asset.ProcessorName = definition.ProcessorName;
            return asset;
        }

        private string GetGlobPattern(BuildConfiguration configuration, AssetDefinition definition)
        {
            // Get an absolute path.
            var globPath = new FilePath(definition.Path.FullPath);
            if (globPath.IsRelative)
            {
                // Combine it with the input directory.
                globPath = configuration.InputDirectory.Combine(globPath);
            }
            else
            {
                // If the path is not relative, then it has to be relative to the input directory.
                var comparison = _environment.FileSystem.IsCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                if (!globPath.FullPath.StartsWith(configuration.InputDirectory.FullPath, comparison))
                {
                    // Invalid glob pattern. Expected asset 'C:/Temp/Hello/World/Text.txt' to be relative to input directory 'C:/Input'.
                    const string format = "Invalid glob pattern. Expected pattern '{0}' to be relative to input directory '{1}'.";
                    var message = string.Format(format, globPath.FullPath, configuration.InputDirectory.FullPath);
                    throw new LuntException(message);
                }
            }
            return globPath.FullPath;
        }
    }
}