namespace Disqord
{
    /// <summary>
    ///     Represent a direct message channel with a user.
    /// </summary>
    public interface IDirectChannel : IPrivateChannel
    {
        /// <summary>
        ///     Gets the recipient of this channel.
        /// </summary>
        IUser Recipient { get; }
    }
}
