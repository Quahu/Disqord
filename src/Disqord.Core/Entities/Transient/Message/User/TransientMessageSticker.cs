using Disqord.Models;

namespace Disqord;

public class TransientMessageSticker : TransientEntity<StickerItemJsonModel>, IMessageSticker
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public StickerFormatType FormatType => Model.FormatType;

    public TransientMessageSticker(StickerItemJsonModel model)
        : base(model)
    { }

    public override string ToString()
    {
        return this.GetString();
    }
}
