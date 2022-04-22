using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class CreateApplicationCommandJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("description")]
        public Optional<string> Description;

        [JsonProperty("options")]
        public Optional<ApplicationCommandOptionJsonModel[]> Options;

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

                    // "CHAT_INPUT command names and command option names must match the following regex ^[\w-]{1,32}$ with the unicode flag set.
                    // If there is a lowercase variant of any letters used, you must use those.
                    // Characters with no lowercase variants and/or uncased letters are still allowed."
                    // ☜(ﾟヮﾟ☜)
                    break;
                }
                case ApplicationCommandType.User:
                case ApplicationCommandType.Message:
                {
                    OptionalGuard.HasNoValue(Description, "Context menu commands must not have descriptions set.");
                    break;
                }
            }

            ContentValidation.ApplicationCommands.ValidateName(Name);
            ContentValidation.ApplicationCommands.ValidateDescription(Description);
            ContentValidation.ApplicationCommands.ValidateOptions(Options);
        }
    }
}
