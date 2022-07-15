using System;
using System.Collections.Generic;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway
{
    /// <inheritdoc cref="IThreadChannel"/>
    public class CachedThreadChannel : CachedMessageGuildChannel, IThreadChannel, IJsonUpdatable<ThreadMembersUpdateJsonModel>, IJsonUpdatable<ThreadMemberJsonModel>
    {
        /// <inheritdoc/>
        public override Snowflake? CategoryId => this.GetChannel()?.CategoryId;

        /// <inheritdoc/>
        public override int Position => this.GetChannel()?.Position ?? 0;

        /// <inheritdoc/>
        public override IReadOnlyList<IOverwrite> Overwrites => this.GetChannel()?.Overwrites ?? Array.Empty<IOverwrite>();

        /// <inheritdoc/>
        public Snowflake ChannelId { get; }

        /// <inheritdoc/>
        public Snowflake CreatorId { get; }

        /// <inheritdoc/>
        public IThreadMember CurrentMember { get; private set; }

        /// <inheritdoc/>
        public int MessageCount { get; private set; }

        /// <inheritdoc/>
        public int MemberCount { get; private set; }

        /// <inheritdoc/>
        public bool IsArchived => _metadata.IsArchived;

        /// <inheritdoc/>
        public TimeSpan AutomaticArchiveDuration => _metadata.AutomaticArchiveDuration;

        /// <inheritdoc/>
        public DateTimeOffset ArchiveStateChangedAt => _metadata.ArchiveStateChangedAt;

        /// <inheritdoc/>
        public bool IsLocked => _metadata.IsLocked;

        /// <inheritdoc/>
        public bool AllowsInvitation => _metadata.AllowsInvitation;

        /// <inheritdoc/>
        public DateTimeOffset? CreatedAt => _metadata.CreatedAt;

        public IThreadMetadata Metadata => _metadata;

        private readonly CachedThreadMetadata _metadata;

        public CachedThreadChannel(IGatewayClient client, ChannelJsonModel model)
            : base(client, model)
        {
            ChannelId = model.ParentId.Value.Value;
            CreatorId = model.OwnerId.Value;

            _metadata = new CachedThreadMetadata(client, model.ThreadMetadata.Value);
        }

        public override void Update(ChannelJsonModel model)
        {
            base.Update(model);

            if (model.Member.HasValue)
                CurrentMember = new TransientThreadMember(Client, model.Member.Value);

            if (model.MessageCount.HasValue)
                MessageCount = model.MessageCount.Value;

            if (model.MemberCount.HasValue)
                MemberCount = model.MemberCount.Value;

            if (_metadata != null && model.ThreadMetadata.HasValue)
                _metadata.Update(model.ThreadMetadata.Value);
        }

        public void Update(ThreadMembersUpdateJsonModel model)
        {
            if (model.AddedMembers.HasValue)
            {
                var memberModel = Array.Find(model.AddedMembers.Value, x => x.UserId.HasValue && x.UserId.Value == Client.CurrentUser.Id);
                if (memberModel != null)
                {
                    CurrentMember = new TransientThreadMember(Client, memberModel);
                    return;
                }
            }

            if (model.RemovedMemberIds.HasValue)
            {
                if (Array.Exists(model.RemovedMemberIds.Value, x => x == Client.CurrentUser.Id))
                    CurrentMember = null;
            }
        }

        public void Update(ThreadMemberJsonModel model)
        {
            CurrentMember = new TransientThreadMember(Client, model);
        }
    }
}
