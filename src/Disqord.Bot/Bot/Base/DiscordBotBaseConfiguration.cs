using System.Collections.Generic;

namespace Disqord.Bot
{
    public class DiscordBotBaseConfiguration
    {
        /// <summary>
        ///     Gets or sets the IDs of the users that own this bot,
        ///     i.e. users who will pass <see cref="DiscordBotBase.IsOwnerAsync"/>.
        /// </summary>
        public IEnumerable<Snowflake> OwnerIds { get; set; }
    }
}
