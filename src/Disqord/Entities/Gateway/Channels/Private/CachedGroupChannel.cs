using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Rest;

namespace Disqord
{
    public sealed class CachedGroupChannel : CachedPrivateChannel, IGroupChannel
    {
        public IReadOnlyDictionary<Snowflake, CachedUser> Recipients => _recipients.ReadOnly();

        public string IconHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        private readonly LockedDictionary<Snowflake, CachedUser> _recipients;

        IReadOnlyDictionary<Snowflake, IUser> IGroupChannel.Recipients => new ReadOnlyUpcastingDictionary<Snowflake, CachedUser, IUser>(Recipients);

        internal CachedGroupChannel(DiscordClientBase client, ChannelModel model) : base(client, model)
        {
            _recipients = new LockedDictionary<Snowflake, CachedUser>(model.Recipients.Value.Length);
            for (var i = 0; i < model.Recipients.Value.Length; i++)
            {
                var recipient = model.Recipients.Value[i];
                _recipients.TryAdd(recipient.Id, client.State.GetOrAddSharedUser(recipient));
            }

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
            => Client.DeleteOrCloseChannelAsync(Id, options);
    }
}
