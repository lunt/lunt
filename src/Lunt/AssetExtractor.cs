using System;
using System.Collections.Generic;
using Lunt.IO;

namespace Lunt
{
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
                    result.AddRange(ExtractGlob(configuration, definition));
                }
                else
                {
                    // Path is a single asset.
                    var path = new FilePath(definition.Path.FullPath);
                    result.Add(CreateAsset(path, definition));
                }
            }
            return result.ToArray();
        }

        private IEnumerable<Asset> ExtractGlob(BuildConfiguration configuration, AssetDefinition definition)
        {
            var pattern = GetGlobPattern(configuration, definition);
            var result = _globber.Glob(pattern);
            foreach (var item in result)
            {
                var inputDirectoryLength = configuration.InputDirectory.FullPath.Length + 1;
                var path = new FilePath(item.FullPath.Substring(inputDirectoryLength));
                yield return CreateAsset(path, definition);
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