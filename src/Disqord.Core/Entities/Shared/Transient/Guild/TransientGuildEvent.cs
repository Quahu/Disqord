using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientGuildEvent : TransientClientEntity<GuildEventJsonModel>, IGuildEvent
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake GuildId => Model.GuildId;

        /// <inheritdoc/>
        public Snowflake? ChannelId => Model.ChannelId;

        /// <inheritdoc/>
        public Snowflake? CreatorId => Model.CreatorId.GetValueOrNullable();

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
        public PrivacyLevel PrivacyLevel => Model.PrivacyLevel;

        /// <inheritdoc/>
        public GuildEventStatus Status => Model.Status;

        /// <inheritdoc/>
        public GuildEventTargetType TargetEntityType => Model.EntityType;

        /// <inheritdoc/>
        public Snowflake? EntityId => Model.EntityId;

        /// <inheritdoc/>
        public IGuildEventMetadata Metadata
        {
            get
            {
                if (Model.EntityMetadata == null)
                    return null;

                return _metadata ??= new TransientGuildEventMetadata(Model.EntityMetadata);
            }
        }
        private IGuildEventMetadata _metadata;

        /// <inheritdoc/>
        public IUser Creator
        {
            get
            {
                if (!Model.Creator.HasValue)
                    return null;

                return _creator ??= new TransientUser(Client, Model.Creator.Value);
            }
        }
        private IUser _creator;

        /// <inheritdoc/>
        public int? MemberCount => Model.UserCount.GetValueOrNullable();

        public TransientGuildEvent(IClient client, GuildEventJsonModel model)
            : base(client, model)
        { }
    }
}
