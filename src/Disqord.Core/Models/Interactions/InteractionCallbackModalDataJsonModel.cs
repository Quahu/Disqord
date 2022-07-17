using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class InteractionCallbackModalDataJsonModel : JsonModel
{
    [JsonProperty("custom_id")]
    public Optional<string> CustomId;

    [JsonProperty("title")]
    public Optional<string> Title;

    [JsonProperty("components")]
    public Optional<ComponentJsonModel[]> Components;

    protected override void OnValidate()
    {
        OptionalGuard.HasValue(CustomId);
        Guard.IsNotNullOrWhiteSpace(CustomId.Value);
        Guard.IsLessThanOrEqualTo(CustomId.Value.Length, Discord.Limits.Interaction.Modal.MaxCustomIdLength);

        OptionalGuard.HasValue(Title);
        Guard.IsNotNullOrWhiteSpace(Title.Value);
        Guard.IsLessThanOrEqualTo(Title.Value.Length, Discord.Limits.Interaction.Modal.MaxTitleLength);

        OptionalGuard.HasValue(Components);
        Guard.IsNotEmpty(Components.Value);
        foreach (var component in Components.Value)
        {
            Guard.IsNotNull(component);
            component.Validate();
        }
    }
}