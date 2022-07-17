using System.Collections.Generic;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

public class DeleteMessagesJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("messages")]
    public IList<Snowflake> Messages = null!;
}
