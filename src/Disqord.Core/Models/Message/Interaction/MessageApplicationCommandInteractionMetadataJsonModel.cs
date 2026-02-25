using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageApplicationCommandInteractionMetadataJsonModel : MessageInteractionMetadataJsonModel
{
    [JsonProperty("name")]
    public Optional<string> Name;

    [JsonProperty("command_type")]
    public Optional<ApplicationCommandType> CommandType;

    [JsonProperty("target_user")]
    public Optional<UserJsonModel> TargetUser;

    [JsonProperty("target_message_id")]
    public Optional<Snowflake> TargetMessageId;
}
