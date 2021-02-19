﻿using System.IO;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyCurrentUserJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("username")]
        public Optional<string> Username;

        [JsonProperty("avatar")]
        public Optional<Stream> Avatar;
    }
}