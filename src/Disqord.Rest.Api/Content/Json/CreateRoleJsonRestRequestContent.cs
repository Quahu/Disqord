﻿using System.IO;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateRoleJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public Optional<string> Name;

    [JsonProperty("permissions")]
    public Optional<Permissions> Permissions;

    [JsonProperty("color")]
    public Optional<int> Color;

    [JsonProperty("colors")]
    public Optional<RoleColorsJsonModel> Colors;

    [JsonProperty("hoist")]
    public Optional<bool> Hoist;

    [JsonProperty("icon")]
    public Optional<Stream> Icon;

    [JsonProperty("mentionable")]
    public Optional<bool> Mentionable;

    [JsonProperty("unicode_emoji")]
    public Optional<string> UnicodeEmoji;
}
