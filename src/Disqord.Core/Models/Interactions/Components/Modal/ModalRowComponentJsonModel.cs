using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ModalRowComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("components")]
    public ModalBaseComponentJsonModel[] Components = null!;

    public ModalRowComponentJsonModel()
    {
        Type = ComponentType.Row;
    }
}
