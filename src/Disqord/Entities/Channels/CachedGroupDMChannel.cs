using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    public sealed class CachedGroupDmChannel : CachedPrivateChannel, IGroupDmChannel
    {
        public IReadOnlyDictionary<Snowflake, CachedUser> Recipients { get; }

        public string IconHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        public CachedUser Owner => Recipients.TryGetValue(OwnerId, out var owner) ? owner : null;

        private readonly ConcurrentDictionary<Snowflake, CachedUser> _recipients;

        IReadOnlyDictionary<Snowflake, IUser> IGroupDmChannel.Recipients => new ReadOnlyUpcastingDictionary<Snowflake, CachedUser, IUser>(Recipients);

        IUser IGroupDmChannel.Owner => Owner;

        internal CachedGroupDmChannel(DiscordClient client, ChannelModel model) : base(client, model)
        {
            _recipients = Extensions.CreateConcurrentDictionary<Snowflake, CachedUser>(model.Recipients.Value.Count);
            for (var i = 0; i < model.Recipients.Value.Count; i++)
            {
                var recipient = model.Recipients.Value[i];
                _recipients.TryAdd(recipient.Id, client.CreateSharedUser(recipient));
            }
            Recipients = new ReadOnlyDictionary<Snowflake, CachedUser>(_recipients);
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            if (model.Icon.HasValue)
                IconHash = model.Icon.Value;

            if (model.OwnerId.HasValue)
                OwnerId = model.OwnerId.Value;

            if (!model.Name.HasValue || string.IsNullOrWhiteSpace(model.Name.Value))
                model.Name = string.Join(", ", Recipients.Values);

            base.Update(model);
        }

        public Task LeaveAsync(RestRequestOptions options = null)
            => Client.RestClient.DeleteOrCloseChannelAsync(Id, options);
    }
}
