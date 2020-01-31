using Disqord.Rest;

namespace Disqord
{
    public abstract class DiscordClientBaseConfiguration : RestDiscordClientConfiguration
    {
        /// <summary>
        ///     Gets or sets the <see cref="Disqord.MessageCache"/> the client should use to cache messages.
        ///     If not set, the client will default to <see cref="DefaultMessageCache"/> with the capacity set to 100.
        /// </summary>
        public Optional<MessageCache> MessageCache { get; set; }

        /// <summary>
        ///     Gets or sets the status the client should set after connecting.
        ///     If not set, the client will default to <see cref="UserStatus.Online"/>.
        /// </summary>
        public Optional<UserStatus> Status { get; set; }

        /// <summary>
        ///     Gets or sets the activity the client should set after connecting.
        ///     If not set, the client will not set any activity.
        /// </summary>
        public Optional<LocalActivity> Activity { get; set; }
    }
}
