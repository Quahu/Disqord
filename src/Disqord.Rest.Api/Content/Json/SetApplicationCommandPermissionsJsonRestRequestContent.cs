﻿using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class SetApplicationCommandPermissionsJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("id")]
        public Optional<Snowflake> Id;

        [JsonProperty("permissions")]
        public ApplicationCommandPermissionsJsonModel[] Permissions;
    }
}
