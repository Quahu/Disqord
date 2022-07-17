using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents a sticker pack.
/// </summary>
public interface IStickerPack : ISnowflakeEntity, INamableEntity
{
    /// <summary>
    ///     Gets the stickers of this sticker pack.
    /// </summary>
    IReadOnlyDictionary<Snowflake, IPackSticker> Stickers { get; }

    /// <summary>
    ///     Gets the ID of the "Game SKU" of this sticker pack.
    /// </summary>
    Snowflake SkuId { get; }

    /// <summary>
    ///     Gets the ID of the cover sticker of this sticker pack.
    ///     Returns <see langword="null"/> if this sticker pack has no cover sticker.
    /// </summary>
    Snowflake? CoverStickerId { get; }

    /// <summary>
    ///     Gets the description of this sticker pack.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the ID of the banner asset of this sticker pack.
    ///     Returns <see langword="null"/> if this sticker pack has no banner.
    /// </summary>
    Snowflake? BannerAssetId { get; }
}