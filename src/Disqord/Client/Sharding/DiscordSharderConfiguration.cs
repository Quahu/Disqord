using System;

namespace Disqord.Sharding
{
    public class DiscordSharderConfiguration : DiscordClientBaseConfiguration
    {
        /// <summary>
        ///     Gets or sets the optional shard count.
        ///     If this is set, the sharder will use it instead of the shard count recommended by Discord.
        /// </summary>
        public Optional<int> ShardCount
        {
            get => shardCount;
            set
            {
                if (value.HasValue && value.Value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                shardCount = value;
            }
        }
        private Optional<int> shardCount;

        /// <summary>
        ///     Instantiates a new <see cref="DiscordSharderConfiguration"/>.
        /// </summary>
        public DiscordSharderConfiguration()
        { }
    }
}
