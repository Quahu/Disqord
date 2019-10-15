using Disqord.Rest;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class TeamMemberModel : UserModel
    {
        [JsonProperty("membership_state")]
        public TeamMembershipState MembershipState { get; set; }

        [JsonProperty("permissions")]
        public string[] Permissions { get; set; }
    }
}