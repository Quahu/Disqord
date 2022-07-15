using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateForumThreadJsonRestRequestContent : CreateThreadJsonRestRequestContent
    {
        [JsonProperty("message")]
        public CreateMessageJsonRestRequestContent Message;
    }
}
