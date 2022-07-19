namespace Disqord;

/// <summary>
///     Represents a guild sticker.
/// </summary>
public interface IGuildSticker : ISticker, IGuildEntity
{
    /// <summary>
    ///     Gets whether this sticker is available.
    /// </summary>
    /// <returns>
    ///     <see langword="false"/> when, for example, the sticker limit has been raised and then lowered.
    /// </returns>
    bool IsAvailable { get; }

    /// <summary>
    ///     Gets the user that created this sticker.
    /// </summary>
    /// <returns>
    ///     <see langword="null"/> when the bot does not have <see cref="Permissions.ManageEmojisAndStickers"/>
    ///     in the guild this sticker belongs to.
    /// </returns>
    IUser Creator { get; }
}
