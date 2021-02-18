using System;
using System.Collections.Generic;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public interface IGatewayGuild : IGuild, IGatewayEntity, IJsonUpdatable<GatewayGuildJsonModel>
    {
        DateTimeOffset JoinedAt { get; }

        bool IsLarge { get; }

        bool IsUnavailable { get; }

        int MemberCount { get; }

        //IReadOnlyList<> VoiceStates { get; }

        IReadOnlyDictionary<Snowflake, IMember> Members { get; }

        IReadOnlyDictionary<Snowflake, IGuildChannel> Channels { get; }

        //IReadOnlyDictionary<> Presences { get; }
    }
}
