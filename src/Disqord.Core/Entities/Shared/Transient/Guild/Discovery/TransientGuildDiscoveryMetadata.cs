using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildDiscoveryMetadata : TransientEntity<GuildDiscoveryMetdataJsonModel>, IGuildDiscoveryMetadata
    {
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public int PrimaryCategoryId => Model.PrimaryCategoryId;

        /// <inheritdoc/>
        public IReadOnlyList<string> Keywords => Model.Keywords;

        /// <inheritdoc/>
        public bool HasEmojis => Model.EmojiDiscoverabilityEnabled;

        /// <inheritdoc/>
        public DateTimeOffset? PartnershipChangedAt => Model.PartnerActionedTimestamp;

        /// <inheritdoc/>
        public DateTimeOffset? PartnershipAppliedAt => Model.PartnerApplicationTimestamp;

        /// <inheritdoc/>
        public IReadOnlyList<int> CategoryIds => Model.CategoryIds;

        public TransientGuildDiscoveryMetadata(IClient client, Snowflake guildId, GuildDiscoveryMetdataJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}