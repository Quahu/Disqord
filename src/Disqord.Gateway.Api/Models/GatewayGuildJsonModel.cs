﻿using System;
using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

[JsonSkippedProperties("lazy")]
public class GatewayGuildJsonModel : GuildJsonModel
{
    [JsonProperty("joined_at")]
    public DateTimeOffset JoinedAt;

    [JsonProperty("large")]
    public bool Large;

    [JsonProperty("unavailable")]
    public Optional<bool> Unavailable;

    [JsonProperty("member_count")]
    public int MemberCount;

    [JsonProperty("voice_states")]
    public VoiceStateJsonModel[] VoiceStates = null!;

    [JsonProperty("members")]
    public MemberJsonModel[] Members = null!;

    [JsonProperty("channels")]
    public ChannelJsonModel[] Channels = null!;

    [JsonProperty("threads")]
    public ChannelJsonModel[] Threads = null!;

    [JsonProperty("presences")]
    public IJsonNode[] Presences = null!;

    [JsonProperty("stage_instances")]
    public StageInstanceJsonModel[] StageInstances = null!;

    [JsonProperty("guild_scheduled_events")]
    public GuildScheduledEventJsonModel[] GuildScheduledEvents = null!;

    // Not ideal - handling the deserialization error at the serializer level would be better
    public IEnumerable<PresenceJsonModel> CreatePresences()
    {
        foreach (var node in Presences)
        {
            PresenceJsonModel? model = null;
            try
            {
                model = node.ToType<PresenceJsonModel>();
            }
            catch
            {
                // Ignore bad presence data.
            }

            if (model == null)
            {
                continue;
            }

            yield return model;
        }
    }
}
