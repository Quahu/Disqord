using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IGuildSticker> ModifyAsync(this IGuildSticker sticker,
        Action<ModifyGuildStickerActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = sticker.GetRestClient();
        return client.ModifyGuildStickerAsync(sticker.GuildId, sticker.Id, action, options, cancellationToken);
    }

    public static Task DeleteAsync(this IGuildSticker sticker,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = sticker.GetRestClient();
        return client.DeleteGuildStickerAsync(sticker.GuildId, sticker.Id, options, cancellationToken);
    }
}