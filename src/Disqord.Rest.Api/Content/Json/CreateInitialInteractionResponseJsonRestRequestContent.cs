using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class CreateInteractionResponseJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("type")]
        public InteractionResponseType Type;

        [JsonProperty("data")]
        public Optional<InteractionCallbackDataJsonModel> Data;
    }
}
