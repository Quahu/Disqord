using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Serialization.Json
{
    public class JsonModel : IJsonObject
    {
        /// <summary>
        ///     Gets or sets an <see cref="IJsonNode"/> as extension data with the given key.
        /// </summary>
        /// <param name="key"> The extension data key. </param>
        public IJsonNode this[string key]
        {
            get => _extensionData?[key];
            set => ExtensionData[key] = value;
        }

        /// <summary>
        ///     Gets the extension data dictionary.
        /// </summary>
        public IDictionary<string, IJsonNode> ExtensionData
        {
            get => _extensionData ??= new Dictionary<string, IJsonNode>();
            set => _extensionData = value;
        }
        private IDictionary<string, IJsonNode> _extensionData;

        int IReadOnlyCollection<KeyValuePair<string, IJsonNode>>.Count => _extensionData?.Count ?? 0;
        IEnumerable<string> IReadOnlyDictionary<string, IJsonNode>.Keys => ExtensionData.Keys;
        IEnumerable<IJsonNode> IReadOnlyDictionary<string, IJsonNode>.Values => ExtensionData.Values;

        T IJsonNode.ToType<T>()
            => throw new NotSupportedException();

        bool IReadOnlyDictionary<string, IJsonNode>.ContainsKey(string key)
            => _extensionData?.ContainsKey(key) ?? false;

        bool IReadOnlyDictionary<string, IJsonNode>.TryGetValue(string key, out IJsonNode value)
        {
            if (_extensionData != null && _extensionData.TryGetValue(key, out value))
                return true;

            value = default;
            return false;
        }

        IEnumerator<KeyValuePair<string, IJsonNode>> IEnumerable<KeyValuePair<string, IJsonNode>>.GetEnumerator()
            => ExtensionData.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ExtensionData.GetEnumerator();
    }
}
