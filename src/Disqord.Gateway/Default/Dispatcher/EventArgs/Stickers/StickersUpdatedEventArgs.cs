using System;
using System.Collections.Generic;

namespace Disqord.Gateway;

public class StickersUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild the stickers were updated in.
    /// </summary>
    public Snowflake GuildId { get; }

    /// <summary>
    ///     Gets the guild the stickers were updated in.
    ///     Returns <see langword="null"/> if the guild was not cached.
    /// </summary>
    public CachedGuild? Guild { get; }

    /// <summary>
    ///     Gets the stickers in the state before the update occurred.
    ///     Returns <see langword="null"/> if the guild was not cached.
    /// </summary>
    public IReadOnlyDictionary<Snowflake, IGuildSticker>? OldStickers { get; }

    /// <summary>
    ///     Gets the updated stickers.
    /// </summary>
    public IReadOnlyDictionary<Snowflake, IGuildSticker> NewStickers { get; }

    public StickersUpdatedEventArgs(
        Snowflake guildId,
        CachedGuild? guild,
        IReadOnlyDictionary<Snowflake, IGuildSticker>? oldStickers,
        IReadOnlyDictionary<Snowflake, IGuildSticker> newStickers)
    {
        GuildId = guildId;
        Guild = guild;
        OldStickers = oldStickers;
        NewStickers = newStickers;
    }
}
