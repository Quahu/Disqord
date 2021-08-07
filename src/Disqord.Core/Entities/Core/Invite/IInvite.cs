﻿using Disqord.Models;

namespace Disqord
{
    /// <summary>
    ///     Represents a channel invite.
    /// </summary>
    public interface IInvite : IChannelEntity, IJsonUpdatable<InviteJsonModel>
    {
        /// <summary>
        ///     Gets the code of this invite.
        /// </summary>
        string Code { get; }

        /// <summary>
        ///     Gets the channel this invite was created for.
        /// </summary>
        IInviteChannel Channel { get; }

        /// <summary>
        ///     Gets the optional user who created this invite.
        /// </summary>
        IUser Inviter { get; }

        // Todo: request parameter specific data
    }
}
