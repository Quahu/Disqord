using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateApplicationCommandJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("name_localizations")]
    public Optional<Dictionary<string, string>?> NameLocalizations;

    [JsonProperty("description")]
    public Optional<string> Description;

    [JsonProperty("description_localizations")]
    public Optional<Dictionary<string, string>?> DescriptionLocalizations;

    [JsonProperty("options")]
    public Optional<ApplicationCommandOptionJsonModel[]> Options;

    [JsonProperty("default_member_permissions")]
    public Optional<Permissions?> DefaultMemberPermissions;

    [JsonProperty("dm_permission")]
    public Optional<bool?> DmPermission;

    [JsonProperty("default_permission")]
    public Optional<bool> DefaultPermission;

    [JsonProperty("type")]
    public Optional<ApplicationCommandType> Type;

    protected override void OnValidate()
    {
        OptionalGuard.HasValue(Type);

        switch (Type.Value)
        {
            case ApplicationCommandType.Slash:
            {
                OptionalGuard.HasValue(Description, "Slash commands must have descriptions set.");
                break;
            }
            case ApplicationCommandType.User:
            case ApplicationCommandType.Message:
            {
                OptionalGuard.HasNoValue(Description, "Context menu commands must not have descriptions set.");
                break;
            }
        }

        RestContentValidation.ApplicationCommands.ValidateName(Name);
        RestContentValidation.ApplicationCommands.ValidateDescription(Description);
        RestContentValidation.ApplicationCommands.ValidateOptions(Options);
    }
}
