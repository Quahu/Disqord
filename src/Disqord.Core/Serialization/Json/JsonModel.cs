using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Serialization.Json
{
    public class JsonModel : IJsonObject
    {
        /// <summary>
        ///     Gets the extension data dictionary.
        /// </summary>
        public IDictionary<string, IJsonNode> ExtensionData
        {
            get => _extensionData ??= new Dictionary<string, IJsonNode>();
            set => _extensionData = value;
        }
        private IDictionary<string, IJsonNode> _extensionData;

        /// <summary>
        ///     Gets or sets an <see cref="IJsonNode"/> as extension data with the given key.
        /// </summary>
        /// <param name="key"> The extension data key. </param>
        public IJsonNode this[string key]
        {
            get => _extensionData?[key];
            set => ExtensionData[key] = value;
        }

        public JsonModel()
        { }

        protected virtual void OnValidate()
        { }

        /// <summary>
        ///     Ensures that this JSON model is valid to be sent to the Discord API.
        /// </summary>
        /// <remarks>
        ///     This method is used internally.
        ///     This is a virtual method that by default does nothing.
        ///     Validation will not exist for all possible models.
        /// </remarks>
        public void Validate()
        {
            try
            {
                OnValidate();
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to validate model {GetType().Name}.", ex);
            }
        }

        // public void Add(string key, IJsonNode value)
        //     => ExtensionData.Add(key, value);

        bool IReadOnlyDictionary<string, IJsonNode>.ContainsKey(string key)
            => _extensionData?.ContainsKey(key) ?? false;

        // bool IDictionary<string, IJsonNode>.Remove(string key)
        //     => _extensionData.Remove(key);

        bool IReadOnlyDictionary<string, IJsonNode>.TryGetValue(string key, out IJsonNode value)
        {
            if (_extensionData != null && _extensionData.TryGetValue(key, out value))
                return true;

            value = default;
            return false;
        }

        // void ICollection<KeyValuePair<string, IJsonNode>>.Add(KeyValuePair<string, IJsonNode> item)
        //     => Add(item.Key, item.Value);
        //
        // void ICollection<KeyValuePair<string, IJsonNode>>.Clear()
        //     => _extensionData?.Clear();
        //
        // bool ICollection<KeyValuePair<string, IJsonNode>>.Contains(KeyValuePair<string, IJsonNode> item)
        //     => _extensionData?.ContainsKey(item.Key) ?? false;
        //
        // void ICollection<KeyValuePair<string, IJsonNode>>.CopyTo(KeyValuePair<string, IJsonNode>[] array, int arrayIndex)
        //     => _extensionData?.CopyTo(array, arrayIndex);
        //
        // bool ICollection<KeyValuePair<string, IJsonNode>>.Remove(KeyValuePair<string, IJsonNode> item)
        //     => _extensionData.Remove(item);
        //
        // bool ICollection<KeyValuePair<string, IJsonNode>>.IsReadOnly => _extensionData.IsReadOnly;
        int IReadOnlyCollection<KeyValuePair<string, IJsonNode>>.Count => _extensionData?.Count ?? 0;

        // ICollection<string> IDictionary<string, IJsonNode>.Keys => _extensionData.Keys;
        // ICollection<IJsonNode> IDictionary<string, IJsonNode>.Values => _extensionData.Values;
        IEnumerable<string> IReadOnlyDictionary<string, IJsonNode>.Keys => _extensionData.Keys;

        IEnumerable<IJsonNode> IReadOnlyDictionary<string, IJsonNode>.Values => _extensionData.Values;

        T IJsonNode.ToType<T>()
            => throw new NotSupportedException();

        IEnumerator<KeyValuePair<string, IJsonNode>> IEnumerable<KeyValuePair<string, IJsonNode>>.GetEnumerator()
            => ExtensionData.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ExtensionData.GetEnumerator();
    }
}
