namespace Disqord
{
    /// <summary>
    ///     Represents a guild's vanity invite.
    /// </summary>
    public interface IVanityInvite 
    {
        /// <summary>
        ///     Gets the code of this vanity invite.
        /// </summary>
        string Code { get; }

        /// <summary>
        ///     Gets the uses of this vanity invite.
        /// </summary>
        int Uses { get; }
    }
}