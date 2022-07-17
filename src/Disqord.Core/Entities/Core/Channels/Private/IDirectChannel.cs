namespace Disqord;

/// <summary>
///     Represent a private direct message channel.
/// </summary>
public interface IDirectChannel : IPrivateChannel
{
    /// <summary>
    ///     Gets the recipient of this channel.
    /// </summary>
    IUser Recipient { get; }
}