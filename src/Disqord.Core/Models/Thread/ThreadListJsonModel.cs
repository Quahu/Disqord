using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ThreadListJsonModel : JsonModel
    {
        [JsonProperty("threads")] 
        public ChannelJsonModel[] Threads;
        
        [JsonProperty("members")] 
        public ThreadMemberJsonModel[] Members;

        [JsonProperty("has_more")] 
        public bool HasMore;
    }
}
