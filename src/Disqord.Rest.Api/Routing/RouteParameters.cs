using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Rest.Api
{
    /// <summary>
    ///     Represents named parameters found in the route.
    /// </summary>
    public sealed class RouteParameters : IEnumerable<KeyValuePair<string, object>>
    {
        /// <summary>
        ///     Gets the major parameter <c>guild_id</c>.
        /// </summary>
        public ulong GuildId => GetParameter<ulong>("guild_id");

        /// <summary>
        ///     Gets the major parameter <c>channel_id</c>.
        /// </summary>
        public ulong ChannelId => GetParameter<ulong>("channel_id");

        /// <summary>
        ///     Gets the major parameter <c>webhook_id</c>.
        /// </summary>
        public ulong WebhookId => GetParameter<ulong>("webhook_id");

        private readonly Dictionary<string, object> _dictionary;

        internal RouteParameters(Dictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        /// <summary>
        ///     Retrieves a parameter with the given name and converts it or retrieves the default value for its type.
        /// </summary>
        /// <typeparam name="T"> The type of the parameter. </typeparam>
        /// <param name="name"> The name of the parameter. </param>
        /// <returns> The value of the parameter or the default value for its type. </returns>
        public T GetParameter<T>(string name)
        {
            if (!_dictionary.TryGetValue(name, out var value))
                return default;

            if (Convert.GetTypeCode(value) == TypeCode.Object)
                return (T) value;

            return (T) Convert.ChangeType(value, typeof(T));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
            => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
