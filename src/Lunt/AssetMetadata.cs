using System;
using System.Collections.Generic;
using System.Linq;

namespace Lunt
{
    /// <summary>
    /// Represents asset metadata.
    /// </summary>
    public sealed class AssetMetadata
    {
        private readonly Dictionary<string, string> _dictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetMetadata" /> class.
        /// </summary>
        /// <param name="dictionary">The metadata.</param>
        /// <exception cref="System.ArgumentNullException">dictionary</exception>
        public AssetMetadata(IDictionary<string, string> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }
            _dictionary = new Dictionary<string, string>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets the number of metadata items.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return _dictionary.Count; }
        }

        /// <summary>
        /// Determines whether the specified key is defined.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key is defined; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDefined(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <returns>The keys.</returns>
        public string[] GetKeys()
        {
            return _dictionary.Keys.ToArray();
        }

        /// <summary>
        /// Gets the value for a specific key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public string GetValue(string key)
        {
            if (IsDefined(key))
            {
                return _dictionary[key];
            }
            return null;
        }
    }
}