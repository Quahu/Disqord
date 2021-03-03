using System.Collections.Generic;
using Disqord.Gateway.Api;

namespace Disqord.Sharding
{
    public interface IDiscordClientSharderConfiguration
    {
        /// <summary>
        ///     Gets or sets the amount of shards this sharder should create.
        ///     If <see langword="null"/>, this will be ignored.
        /// </summary>
        /// <remarks>
        ///     This property is mutually exclusive with <see cref="ShardIds"/>.
        /// </remarks>
        int? ShardCount { get; set; }

        /// <summary>
        ///     Gets or sets the shards this sharder should manage.
        ///     If <see langword="null"/>, this will be ignored.
        /// </summary>
        /// <remarks>
        ///     This property is mutually exclusive with <see cref="ShardCount"/>.
        /// </remarks>
        IEnumerable<ShardId> ShardIds { get; set; }
    }
}
