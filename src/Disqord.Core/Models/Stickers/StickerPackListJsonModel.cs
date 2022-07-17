using Disqord.Serialization.Json;

namespace Disqord.Models;

public class StickerPackListJsonModel : JsonModel
{
    [JsonProperty("sticker_packs")]
    public StickerPackJsonModel[] StickerPacks = null!;
}
