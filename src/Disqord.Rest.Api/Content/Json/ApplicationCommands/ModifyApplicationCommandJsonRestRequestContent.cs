using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class ModifyApplicationCommandJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("default_permission")]
        public Optional<bool> DefaultPermission;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;

        protected override void OnValidate()
        {
            OptionalGuard.CheckValue(Name, value => ContentValidation.ApplicationCommands.ValidateName(value));
            ContentValidation.ApplicationCommands.ValidateDescription(Description);
            ContentValidation.ApplicationCommands.ValidateOptions(Options);
        }
    }
}
