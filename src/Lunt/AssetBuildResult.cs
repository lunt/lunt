using System;

namespace Lunt
{
    /// <summary>
    /// Represents the build result of an asset.B
    /// </summary>
    public sealed class AssetBuildResult
    {
        private readonly BuildManifestItem _manifestItem;
        private readonly Message _message;

        /// <summary>
        /// The asset that was built.
        /// </summary>
        /// <value>The asset.</value>
        public Asset Asset
        {
            get { return _manifestItem.Asset; }
        }

        /// <summary>
        /// Gets the asset build status.
        /// </summary>
        /// <value>The status.</value>
        public AssetBuildStatus Status
        {
            get { return _manifestItem.Status; }
        }

        /// <summary>
        /// Gets the built manifest item.
        /// </summary>
        /// <value>The manifest.</value>
        public BuildManifestItem ManifestItem
        {
            get { return _manifestItem; }
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>The message.</value>
        public Message Message
        {
            get { return _message; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetBuildResult" /> class.
        /// </summary>
        /// <param name="manifestItem">The manifest item.</param>
        /// <param name="message">The build result message.</param>
        /// <exception cref="System.ArgumentNullException">manifestItem</exception>
        public AssetBuildResult(BuildManifestItem manifestItem, Message message)
        {
            if (manifestItem == null)
            {
                throw new ArgumentNullException("manifestItem");
            }
            _manifestItem = manifestItem;
            _message = message ?? Message.Empty;
        }

        /// <summary>
        /// Creates a build result representing a successfull build.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <returns>A build result representing a successfull build.</returns>
        internal static AssetBuildResult Success(BuildManifestItem manifest)
        {
            manifest.Status = AssetBuildStatus.Success;
            manifest.Message = string.Empty;
            return new AssetBuildResult(manifest, Message.Empty);
        }

        /// <summary>
        /// Creates a build result representing a failed build.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        /// <param name="message">The message.</param>
        /// <returns>A build result representing a failed build.</returns>
        internal static AssetBuildResult Failure(BuildManifestItem manifest, Message message)
        {
            manifest.Status = AssetBuildStatus.Failure;
            manifest.Message = message.ToString();
            return new AssetBuildResult(manifest, message);
        }
    }
}