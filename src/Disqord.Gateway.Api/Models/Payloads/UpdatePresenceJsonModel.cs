﻿using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models
{
    public class UpdatePresenceJsonModel : JsonModel
    {
        [JsonProperty("since")]
        public int? Since;

        [JsonProperty("activities")]
        public ActivityJsonModel[] Activities;

        [JsonProperty("status")]
        public UserStatus Status;

        [JsonProperty("afk")]
        public bool Afk;
    }
}