using Disqord.Logging;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    public class RestDiscordClientConfiguration
    {
        /// <summary>
        ///     Gets or sets the logger the client should use.
        /// </summary>
        public Optional<ILogger> Logger { get; set; }

        /// <summary>
        ///     Gets or sets the JSON serializer the client should use.
        /// </summary>
        public Optional<IJsonSerializer> Serializer { get; set; }

        /// <summary>
        ///     Gets or sets the default mentions for messages.
        /// </summary>
        public Optional<LocalMentions> DefaultMentions { get; set; }

        /// <summary>
        ///     Gets or sets the default request options.
        /// </summary>
        public Optional<RestRequestOptions> DefaultRequestOptions { get; set; }
    }
}
