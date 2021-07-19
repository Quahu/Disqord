namespace Disqord
{
    /// <summary>
    ///     Represents a guild sticker.
    /// </summary>
    public interface IGuildSticker : ISticker, IGuildEntity
    {
        /// <summary>
        ///     Gets whether this sticker is available.
        ///     Returns <see langword="false"/> when, for example, the sticker limit has been raised and then lowered.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        ///     Gets the user that created this sticker.
        ///     Returns <see langword="null"/> when the bot does not have <see cref="Permission.ManageEmojisAndStickers"/>
        ///     in the guild which this sticker belongs to.
        /// </summary>
        IUser Creator { get; }
    }
}