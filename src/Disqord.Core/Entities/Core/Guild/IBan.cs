namespace Disqord
{
    /// <summary>
    ///     Represents a guild ban.
    /// </summary>
    public interface IBan : IClientEntity, IGuildEntity
    {
        /// <summary>
        ///     Gets the user of this ban.
        /// </summary>
        IUser User { get; }

        /// <summary>
        ///     Gets the reason of this ban.
        /// </summary>
        string Reason { get; }
    }
}
