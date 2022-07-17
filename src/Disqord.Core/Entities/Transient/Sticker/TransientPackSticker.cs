using Disqord.Models;

namespace Disqord;

public class TransientPackSticker : TransientSticker, IPackSticker
{
    /// <inheritdoc/>
    public Snowflake PackId => Model.PackId.Value;

    /// <inheritdoc/>
    public int Position => Model.SortValue.Value;

    public TransientPackSticker(IClient client, StickerJsonModel model)
        : base(client, model)
    { }
}