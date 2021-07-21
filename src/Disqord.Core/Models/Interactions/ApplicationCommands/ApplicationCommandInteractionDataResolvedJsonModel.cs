﻿using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationCommandInteractionDataResolvedJsonModel : JsonModel
    {
        [JsonProperty("users")]
        public Optional<Dictionary<Snowflake, UserJsonModel>> Users;

        [JsonProperty("members")]
        public Optional<Dictionary<Snowflake, MemberJsonModel>> Members;

        [JsonProperty("roles")]
        public Optional<Dictionary<Snowflake, RoleJsonModel>> Roles;

        [JsonProperty("channels")]
        public Optional<Dictionary<Snowflake, ChannelJsonModel>> Channels;
    }
}
