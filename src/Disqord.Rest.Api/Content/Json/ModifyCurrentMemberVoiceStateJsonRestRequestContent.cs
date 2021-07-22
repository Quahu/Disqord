using System;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyCurrentMemberVoiceStateJsonRestRequestContent : ModifyMemberVoiceStateJsonRestRequestContent
    {
        [JsonProperty("request_to_speak_timestamp")]
        public Optional<DateTimeOffset?> RequestToSpeakTimestamp;
    }
}
