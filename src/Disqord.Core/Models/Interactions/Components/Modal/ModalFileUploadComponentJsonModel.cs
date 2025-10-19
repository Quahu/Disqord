using Disqord.Serialization.Json;

namespace Disqord.Models;

public class ModalFileUploadComponentJsonModel : ModalBaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("values")]
    public string[] Values = null!;
}
