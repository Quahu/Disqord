using System;
using System.Collections.Generic;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    public interface IGatewayGuild : IGuild, IGatewayClientEntity, IJsonUpdatable<GatewayGuildJsonModel>
    {
        /// <summary>
        ///     Gets when the bot joined this guild.
        /// </summary>
        DateTimeOffset JoinedAt { get; }

        /// <summary>
        ///     Gets whether this guild is considered large.
        /// </summary>
        bool IsLarge { get; }

        /// <summary>
        ///     Gets whether this guild is unavailable.
        /// </summary>
        bool IsUnavailable { get; }

        /// <summary>
        ///     Gets the member count of this guild.
        /// </summary>
        int MemberCount { get; }

        /// <summary>
        ///     Gets the members of this guild keyed by the IDs of the members.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IMember> Members { get; }

        /// <summary>
        ///     Gets the channels of this guild keyed by the IDs of the channels.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IGuildChannel> Channels { get; }

        /// <summary>
        ///     Gets the voice states of this guild keyed by the IDs of the members.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IVoiceState> VoiceStates { get; }

        /// <summary>
        ///     Gets the presences of this guild keyed by the IDs of the members.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IPresence> Presences { get; }

        /// <summary>
        ///     Gets the stages of this guild keyed by the IDs of the stages.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IStage> Stages { get; }

        /// <summary>
        ///     Gets the guild events of this guild keyed by the IDs of the events.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IGuildEvent> GuildEvents { get; }
    }
}
