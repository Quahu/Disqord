using System;
using System.Collections.Generic;
using Disqord.Models;

namespace Disqord
{
    public class TransientThreadChannel : TransientMessageGuildChannel, IThreadChannel
    {
        /// <inheritdoc/>
        public override Snowflake? CategoryId => null;

        /// <inheritdoc/>
        public override int Position => 0;

        /// <inheritdoc/>
        public override IReadOnlyList<IOverwrite> Overwrites => Array.Empty<IOverwrite>();

        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ParentId.Value.Value;

        /// <inheritdoc/>
        public Snowflake CreatorId => Model.OwnerId.Value;

        /// <inheritdoc/>
        public IThreadMember CurrentMember
        {
            get
            {
                if (!Model.Member.HasValue)
                    return null;

                return _currentMember ??= new TransientThreadMember(Client, Model.Member.Value);
            }
        }
        private TransientThreadMember _currentMember;

        /// <inheritdoc/>
        public int MessageCount => Model.MessageCount.Value;

        /// <inheritdoc/>
        public int MemberCount => Model.MemberCount.Value;

        /// <inheritdoc/>
        public bool IsArchived => Model.ThreadMetadata.Value.Archived;

        /// <inheritdoc/>
        public TimeSpan AutomaticArchiveDuration => TimeSpan.FromMinutes(Model.ThreadMetadata.Value.AutoArchiveDuration);

        /// <inheritdoc/>
        public DateTimeOffset ArchiveStateChangedAt => Model.ThreadMetadata.Value.ArchiveTimestamp;

        /// <inheritdoc/>
        public bool IsLocked => Model.ThreadMetadata.Value.Locked.GetValueOrDefault();

        /// <inheritdoc/>
        public string Mention => Disqord.Mention.TextChannel(Id);

        /// <inheritdoc/>
        public string Tag => $"#{Name}";

        public TransientThreadChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
