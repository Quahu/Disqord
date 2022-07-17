using Disqord.Models;

namespace Disqord;

public class TransientSticker : TransientClientEntity<StickerJsonModel>, ISticker
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public StickerFormatType FormatType => Model.FormatType;

    /// <inheritdoc/>
    public string? Description => Model.Description;

    /// <inheritdoc/>
    public string Tags => Model.Tags;

    /// <inheritdoc/>
    public StickerType Type => Model.Type;

    public TransientSticker(IClient client, StickerJsonModel model)
        : base(client, model)
    { }

    public static ISticker Create(IClient client, StickerJsonModel model)
        => model.Type switch
        {
            StickerType.Pack => new TransientPackSticker(client, model),
            StickerType.Guild => new TransientGuildSticker(client, model),
            _ => new TransientSticker(client, model)
        };

    public override string ToString()
    {
        return this.GetString();
    }
}
