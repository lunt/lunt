using System.Collections.Generic;

namespace Lunt.Testing
{
    public static class BuildManifestHelper
    {
        public static BuildManifestItem CloneWithoutMetadata(BuildManifestItem manifestItem)
        {
            return CloneWithMetadata(manifestItem, new Dictionary<string, string>());
        }

        public static BuildManifestItem CloneWithMetadata(BuildManifestItem manifestItem, IDictionary<string, string> metadata)
        {
            var asset = new Asset(manifestItem.Asset.Path, metadata);
            var item = new BuildManifestItem(asset);
            item.Checksum = manifestItem.Checksum;
            item.Length = manifestItem.Length;
            item.Message = manifestItem.Message;
            item.Status = manifestItem.Status;
            return item;
        }
    }
}