using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Bot.Commands.Application.Default;

public partial class DefaultApplicationCommandLocalizer
{
    public class LocalizationStoreJsonModel : JsonModel
    {
        [JsonProperty("schema_version")]
        public int SchemaVersion;

        [JsonProperty("global_localizations")]
        public LocalizationNodeJsonModel? GlobalLocalizations;

        [JsonProperty("guild_localizations")]
        public Dictionary<Snowflake, LocalizationNodeJsonModel>? GuildLocalizations;
    }

    public class LocalizationNodeJsonModel : JsonModel
    {
        [JsonProperty("context_menu_commands")]
        public Optional<Dictionary<string, CommandLocalizationJsonModel?>?> ContextMenuCommands;

        [JsonProperty("slash_commands")]
        public Optional<Dictionary<string, CommandLocalizationJsonModel?>?> SlashCommands;
    }

    public class CommandLocalizationJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string? Name;

        [JsonProperty("description")]
        public Optional<string?> Description;

        [JsonProperty("options")]
        public Optional<Dictionary<string, OptionLocalizationJsonModel?>?> Options;
    }

    public class OptionLocalizationJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string? Name;

        [JsonProperty("description")]
        public string? Description;

        [JsonProperty("options")]
        public Optional<Dictionary<string, OptionLocalizationJsonModel?>?> Options;

        [JsonProperty("choices")]
        public Optional<Dictionary<string, ChoiceLocalizationJsonModel?>?> Choices;
    }

    public class ChoiceLocalizationJsonModel : JsonModel
    {
        [JsonProperty("name")]
        public string? Name;
    }
}
