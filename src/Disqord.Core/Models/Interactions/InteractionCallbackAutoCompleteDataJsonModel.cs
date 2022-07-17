using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class InteractionCallbackAutoCompleteDataJsonModel : JsonModel
{
    [JsonProperty("choices")]
    public Optional<ApplicationCommandOptionChoiceJsonModel[]> Choices;

    protected override void OnValidate()
    {
        OptionalGuard.CheckValue(Choices, value =>
        {
            Guard.IsNotNull(value);
            Guard.HasSizeLessThanOrEqualTo(value, Discord.Limits.ApplicationCommand.Option.MaxChoiceAmount);

            foreach (var choice in value)
            {
                Guard.IsNotNull(choice);
                choice.Validate();
            }
        });
    }
}