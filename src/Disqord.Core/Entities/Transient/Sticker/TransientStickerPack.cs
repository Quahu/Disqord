using System.Collections.Generic;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord;

public class TransientStickerPack : TransientClientEntity<StickerPackJsonModel>, IStickerPack
{
    /// <inheritdoc/>
    public Snowflake Id => Model.Id;

    /// <inheritdoc cref="INamableEntity.Name"/>
    public string Name => Model.Name;

    /// <inheritdoc/>
    public IReadOnlyDictionary<Snowflake, IPackSticker> Stickers => _stickers ??= Model.Stickers.ToReadOnlyDictionary(Client,
        (model, _) => model.Id, (model, client) => new TransientPackSticker(client, model) as IPackSticker);

    private IReadOnlyDictionary<Snowflake, IPackSticker>? _stickers;

    /// <inheritdoc/>
    public Snowflake SkuId => Model.SkuId;

    /// <inheritdoc/>
    public Snowflake? CoverStickerId => Model.CoverStickerId.GetValueOrNullable();

    /// <inheritdoc/>
    public string Description => Model.Description;

    /// <inheritdoc/>
    public Snowflake? BannerAssetId => Model.BannerAssetId.GetValueOrNullable();

    public TransientStickerPack(IClient client, StickerPackJsonModel model)
        : base(client, model)
    { }
}
