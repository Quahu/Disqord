using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a nitro sticker pack.
    /// </summary>
    public interface IStickerPack : ISnowflakeEntity, INamable
    {
        /// <summary>
        ///     Gets the stickers of this sticker pack.
        /// </summary>
        IReadOnlyDictionary<Snowflake, IPackSticker> Stickers { get; }

        /// <summary>
        ///     Gets the sku ID of this sticker pack.
        /// </summary>
        Snowflake SkuId { get; }

        /// <summary>
        ///     Gets the cover sticker ID of this sticker pack.
        /// </summary>
        Snowflake? CoverStickerId { get; }

        /// <summary>
        ///     Gets the description of this sticker pack.
        /// </summary>
        string Description { get; }

        /// <summary>
        ///     Gets the banner asset ID of this sticker pack.
        /// </summary>
        Snowflake? BannerAssetId { get; }
    }
}
