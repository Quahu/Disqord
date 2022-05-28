using System.Collections.Generic;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class ModifyApplicationCommandJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("name_localizations")]
        public Optional<Dictionary<string, string>> NameLocalizations;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("description_localizations")]
        public Optional<Dictionary<string, string>> DescriptionLocalizations;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;

        [JsonProperty("default_member_permissions")]
        public Optional<ulong?> DefaultMemberPermissions;

        [JsonProperty("default_permission")]
        public Optional<bool> DefaultPermission;

        protected override void OnValidate()
        {
            OptionalGuard.CheckValue(Name, value => RestContentValidation.ApplicationCommands.ValidateName(value));
            RestContentValidation.ApplicationCommands.ValidateDescription(Description);
            RestContentValidation.ApplicationCommands.ValidateOptions(Options);
        }
    }
}
