using System;

namespace Disqord.Gateway
{
    public class InviteCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the optional ID of the guild in which the invite was created.
        /// </summary>
        public Optional<Snowflake> GuildId { get; }

        /// <summary>
        ///     Gets the ID of the channel the invite was created for.
        /// </summary>
        public Snowflake ChannelId => Invite.ChannelId;

        /// <summary>
        ///     Gets the code of the created invite.
        /// </summary>
        public string Code => Invite.Code;

        /// <summary>
        ///     Gets the created invite.
        /// </summary>
        public IInvite Invite { get; }

        public InviteCreatedEventArgs(
            Optional<Snowflake> guildId,
            IInvite invite)
        {
            GuildId = guildId;
            Invite = invite;
        }
    }
}
