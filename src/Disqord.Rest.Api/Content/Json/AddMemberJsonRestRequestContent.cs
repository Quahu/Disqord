using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class AddMemberJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("access_token")]
        public string AccessToken;

        [JsonProperty("nick")]
        public Optional<string> Nick;

        [JsonProperty("roles")]
        public Optional<Snowflake[]> Roles;

        [JsonProperty("mute")]
        public Optional<bool> Mute;

        [JsonProperty("deaf")]
        public Optional<bool> Deaf;

        public AddMemberJsonRestRequestContent(string accessToken)
        {
            AccessToken = accessToken;
        }
    }
}
