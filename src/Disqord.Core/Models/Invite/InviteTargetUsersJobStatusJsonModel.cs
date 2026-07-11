using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class InviteTargetUsersJobStatusJsonModel : JsonModel
{
    [JsonProperty("status")]
    public InviteTargetUsersJobStatus Status;

    [JsonProperty("total_users")]
    public int TotalUsers;

    [JsonProperty("processed_users")]
    public int ProcessedUsers;

    [JsonProperty("created_at")]
    public DateTimeOffset? CreatedAt;

    [JsonProperty("completed_at")]
    public DateTimeOffset? CompletedAt;

    [JsonProperty("error_message")]
    public Optional<string> ErrorMessage;
}
