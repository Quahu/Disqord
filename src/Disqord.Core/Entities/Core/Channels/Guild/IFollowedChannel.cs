using Disqord.Models;

namespace Disqord
{
    public interface IFollowedChannel : IEntity, IJsonUpdatable<FollowedChannelJsonModel>
    {
        /// <summary>
        ///     Gets the ID of the source channel of this followed channel.
        /// </summary>
        public Snowflake ChannelId { get; }

        /// <summary>
        ///     Gets the ID of the created target webhook of this followed channel.
        /// </summary>
        public Snowflake WebhookId { get; }
    }
}
