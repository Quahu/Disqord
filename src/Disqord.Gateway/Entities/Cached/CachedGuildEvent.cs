using System;
using Disqord.Models;

namespace Disqord.Gateway
{
    /// <inheritdoc cref="IGuildEvent"/>
    public class CachedGuildEvent : CachedSnowflakeEntity, IGuildEvent
    {
        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public Snowflake? ChannelId { get; private set; }

        /// <inheritdoc/>
        public Snowflake? CreatorId { get; private set; }

        /// <inheritdoc/>
        public IUser Creator { get; private set; }

        /// <inheritdoc/>
        public string Name { get; private set; }

        /// <inheritdoc/>
        public string Description { get; private set; }

        /// <inheritdoc/>
        public string ImageHash { get; private set; }

        /// <inheritdoc/>
        public DateTimeOffset StartsAt { get; private set; }

        /// <inheritdoc/>
        public DateTimeOffset? EndsAt { get; private set; }

        /// <inheritdoc/>
        public PrivacyLevel PrivacyLevel { get; private set; }

        /// <inheritdoc/>
        public GuildEventStatus Status { get; private set; }

        /// <inheritdoc/>
        public GuildEventTargetType TargetType { get; private set; }

        /// <inheritdoc/>
        public Snowflake? TargetId { get; private set; }

        /// <inheritdoc/>
        public IGuildEventMetadata Metadata { get; private set; }

        /// <inheritdoc/>
        public int? MemberCount { get; private set; }

        public CachedGuildEvent(IGatewayClient client, GuildScheduledEventJsonModel model)
            : base(client, model.Id)
        {
            GuildId = model.GuildId;

            Update(model);
        }

        public void Update(GuildScheduledEventJsonModel model)
        {
            if (model.CreatorId.HasValue)
                CreatorId = model.CreatorId.Value;

            if (model.Creator.HasValue)
                Creator = new TransientUser(Client, model.Creator.Value);

            ChannelId = model.ChannelId;
            Name = model.Name;
            Description = model.Description.Value;
            ImageHash = model.Image;
            StartsAt = model.ScheduledStartTime;
            EndsAt = model.ScheduledEndTime;
            PrivacyLevel = model.PrivacyLevel;
            Status = model.Status;
            TargetType = model.EntityType;
            TargetId = model.EntityId;

            if (model.EntityMetadata != null)
                Metadata = new TransientGuildEventMetadata(model.EntityMetadata);

            MemberCount = model.UserCount.GetValueOrNullable();
        }
    }
}
