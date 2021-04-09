using System;
using System.Collections.Generic;

namespace Disqord.Serialization.Json
{
    public class JsonModel : IJsonObject
    {
        /// <summary>
        ///     Gets or sets an <see cref="IJsonToken"/> as extension data with the given key.
        /// </summary>
        /// <param name="key"> The extension data key. </param>
        public IJsonToken this[string key]
        {
            get => _extensionData?[key];
            set => ExtensionData[key] = value;
        }

        /// <summary>
        ///     Gets the extension data dictionary.
        /// </summary>
        public IDictionary<string, IJsonToken> ExtensionData
        {
            get
            {
                if (_extensionData == null)
                    _extensionData = new Dictionary<string, IJsonToken>();

                return _extensionData;
            }
            set => _extensionData = value;
        }
        private IDictionary<string, IJsonToken> _extensionData;

        T IJsonToken.ToType<T>()
            => throw new NotSupportedException();
    }
}
