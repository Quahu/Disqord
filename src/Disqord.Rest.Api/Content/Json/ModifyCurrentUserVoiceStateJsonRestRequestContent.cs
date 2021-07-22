using System;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyCurrentUserVoiceStateJsonRestRequestContent : ModifyUserVoiceStateJsonRestRequestContent
    {
        [JsonProperty("request_to_speak_timestamp", NullValueHandling.Ignore)]
        public Optional<DateTimeOffset?> RequestToSpeakTimestamp;
    }
}
