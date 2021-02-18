using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a channel invite.
    /// </summary>
    public interface IInvite : IEntity, IChannelEntity, IJsonUpdatable<InviteJsonModel>
    {
        /// <summary>
        ///     Gets the code of this invite.
        /// </summary>
        string Code { get; }

        /// <summary>
        ///     Gets the optional metadata of this invite.
        /// </summary>
        IInviteMetadata Metadata { get; }
    }
}