using System;
using System.Collections.Generic;
using Disqord.Collections;
using Disqord.Gateway;
using Disqord.Gateway.Api.Models;

namespace Disqord
{
    public class TransientGatewayGuild : TransientGuild, IGatewayGuild, ITransientEntity<GatewayGuildJsonModel>
    {
        public DateTimeOffset JoinedAt => Model.JoinedAt;

        public bool IsLarge => Model.Large;

        public bool IsUnavailable => Model.Unavailable.GetValueOrDefault();

        public int MemberCount => Model.MemberCount;

        public IReadOnlyDictionary<Snowflake, IMember> Members
        {
            get
            {
                if (_members == null)
                    _members = Model.Members.ToReadOnlyDictionary((Client, Id), (x, _) => x.User.Value.Id, (x, tuple) =>
                    {
                        var (client, guildId) = tuple;
                        return new TransientMember(client, guildId, x) as IMember;
                    });

                return _members;
            }
        }
        private IReadOnlyDictionary<Snowflake, IMember> _members;

        public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels
        {
            get
            {
                if (_channels == null)
                    _channels = Model.Channels.ToReadOnlyDictionary(Client, (x, _) => x.Id, (x, client) => TransientGuildChannel.Create(client, x) as IGuildChannel);

                return _channels;
            }
        }
        private IReadOnlyDictionary<Snowflake, IGuildChannel> _channels;

        public new GatewayGuildJsonModel Model => base.Model as GatewayGuildJsonModel;

        public new IGatewayClient Client => base.Client as IGatewayClient;

        public TransientGatewayGuild(IClient client, GatewayGuildJsonModel model)
            : base(client, model)
        { }
    }
}
