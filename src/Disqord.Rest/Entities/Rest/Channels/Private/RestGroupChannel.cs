using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestGroupChannel : RestPrivateChannel, IGroupChannel
    {
        public string IconHash { get; private set; }

        public Snowflake OwnerId { get; private set; }

        public IReadOnlyDictionary<Snowflake, RestUser> Recipients { get; private set; }

        IReadOnlyDictionary<Snowflake, IUser> IGroupChannel.Recipients => new ReadOnlyUpcastingDictionary<Snowflake, RestUser, IUser>(Recipients);

        internal RestGroupChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            Recipients = model.Recipients.Value.ToDictionary(
                x => new Snowflake(x.Id), x => new RestUser(Client, x)).ReadOnly();
            IconHash = model.Icon.Value;
            OwnerId = model.OwnerId.Value;

            if (!model.Name.HasValue || string.IsNullOrWhiteSpace(model.Name.Value))
                model.Name = string.Join(", ", Recipients.Values);

            base.Update(model);
        }

        public Task LeaveAsync(RestRequestOptions options = null)
            => Client.DeleteOrCloseChannelAsync(Id, options);
    }
}
