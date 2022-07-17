using System.IO;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyWebhookJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public Optional<string> Name;

    [JsonProperty("avatar")]
    public Optional<Stream> Avatar;

    [JsonProperty("channel_id")]
    public Optional<Snowflake> ChannelId;
}