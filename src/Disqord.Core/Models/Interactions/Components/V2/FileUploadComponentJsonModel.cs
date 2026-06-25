using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class FileUploadComponentJsonModel : BaseComponentJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("min_values")]
    public Optional<int> MinValues;

    [JsonProperty("max_values")]
    public Optional<int> MaxValues;

    [JsonProperty("required")]
    public Optional<bool> Required;

    [JsonProperty("file_types")]
    public Optional<string[]> FileTypes;

    public FileUploadComponentJsonModel()
    {
        Type = ComponentType.FileUpload;
    }
}
