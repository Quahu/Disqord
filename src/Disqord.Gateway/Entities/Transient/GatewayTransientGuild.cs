using System;
using System.Collections.Generic;
using Disqord.Gateway;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord
{
    public class GatewayTransientGuild : TransientGuild, IGatewayGuild, ITransientEntity<GatewayGuildJsonModel>
    {
        public DateTimeOffset JoinedAt { get; }

        public bool IsLarge { get; }

        public bool IsUnavailable { get; }

        public int MemberCount { get; }

        public IReadOnlyDictionary<Snowflake, IMember> Members { get; }

        public IReadOnlyDictionary<Snowflake, IGuildChannel> Channels { get; }

        public new GatewayGuildJsonModel Model => base.Model as GatewayGuildJsonModel;

        public new IGatewayClient Client => base.Client as IGatewayClient;

        public GatewayTransientGuild(IClient client, GuildJsonModel model) : base(client, model)
        {

        }
    }
}
