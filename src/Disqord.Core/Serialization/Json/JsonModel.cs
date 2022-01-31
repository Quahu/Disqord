using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Disqord.Serialization.Json
{
    /// <summary>
    ///     Represents a JSON model.
    /// </summary>
    public class JsonModel : IJsonObject
    {
        /// <summary>
        ///     The runtime cache for extension data dictionaries.
        /// </summary>
        public static readonly ConditionalWeakTable<JsonModel, IDictionary<string, IJsonNode>> ExtensionDataCache = new();

        /// <summary>
        ///     Gets the extension data dictionary.
        /// </summary>
        public IDictionary<string, IJsonNode> ExtensionData
        {
            get => ExtensionDataCache.GetValue(this, _ => new Dictionary<string, IJsonNode>());
            set => ExtensionDataCache.AddOrUpdate(this, value);
        }

        /// <summary>
        ///     Gets or sets an <see cref="IJsonNode"/> as extension data with the given key.
        /// </summary>
        /// <param name="key"> The extension data key. </param>
        public IJsonNode this[string key]
        {
            get => ExtensionDataCache.TryGetValue(this, out var extensionData) ? extensionData[key] : null;
            set => ExtensionData[key] = value;
        }

        /// <summary>
        ///     Instantiates a new <see cref="JsonModel"/>.
        /// </summary>
        public JsonModel()
        { }

        /// <inheritdoc cref="Validate"/>
        /// <remarks>
        ///     This is called by <see cref="Validate"/>.
        /// </remarks>
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
            => ExtensionDataCache.TryGetValue(this, out var extensionData) && extensionData.ContainsKey(key);

        // bool IDictionary<string, IJsonNode>.Remove(string key)
        //     => _extensionData.Remove(key);

        bool IReadOnlyDictionary<string, IJsonNode>.TryGetValue(string key, out IJsonNode value)
        {
            if (ExtensionDataCache.TryGetValue(this, out var extensionData) && extensionData.TryGetValue(key, out value))
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
        int IReadOnlyCollection<KeyValuePair<string, IJsonNode>>.Count => ExtensionDataCache.TryGetValue(this, out var extensionData) ? extensionData.Count : 0;

        // ICollection<string> IDictionary<string, IJsonNode>.Keys => _extensionData.Keys;
        // ICollection<IJsonNode> IDictionary<string, IJsonNode>.Values => _extensionData.Values;
        IEnumerable<string> IReadOnlyDictionary<string, IJsonNode>.Keys => ExtensionData.Keys;

        IEnumerable<IJsonNode> IReadOnlyDictionary<string, IJsonNode>.Values => ExtensionData.Values;

        T IJsonNode.ToType<T>()
            => throw new NotSupportedException();

        IEnumerator<KeyValuePair<string, IJsonNode>> IEnumerable<KeyValuePair<string, IJsonNode>>.GetEnumerator()
            => ExtensionData.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ExtensionData.GetEnumerator();
    }
}
