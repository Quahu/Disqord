using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildEvent : TransientEntity<GuildEventJsonModel>, IGuildEvent
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake GuildId => Model.GuildId;

        /// <inheritdoc/>
        public Snowflake? ChannelId => Model.ChannelId;

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string Description => Model.Description.GetValueOrDefault();

        /// <inheritdoc/>
        public string ImageHash => Model.Image;

        /// <inheritdoc/>
        public DateTimeOffset StartTime => Model.ScheduledStartTime;

        /// <inheritdoc/>
        public DateTimeOffset? EndTime => Model.ScheduledEndTime;

        /// <inheritdoc/>
        public StagePrivacyLevel PrivacyLevel => Model.PrivacyLevel;

        /// <inheritdoc/>
        public GuildEventStatus Status => Model.Status;

        /// <inheritdoc/>
        public GuildEventTarget EntityType => Model.EntityType;

        /// <inheritdoc/>
        public Snowflake? EntityId => Model.EntityId;

        /// <inheritdoc/>
        public IReadOnlyList<Snowflake> SkuIds => Model.SkuIds.ToReadOnlyList();

        /// <inheritdoc/>
        public int? UserCount => Model.UserCount.GetValueOrNullable();

        public IReadOnlyList<Snowflake> SpeakerIds => Optional.ConvertOrDefault(Model.EntityMetadata.SpeakerIds, x => x.ToReadOnlyList(), ReadOnlyList<Snowflake>.Empty);

        public TransientGuildEvent(IClient client, GuildEventJsonModel model)
            : base(client, model)
        { }
    }
}
