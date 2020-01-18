namespace Disqord.Events
{
    /// <summary>
    ///     Represents the arguments for the <see cref="DiscordClientBase.InviteDeleted"/> event.
    /// </summary>
    public sealed class InviteDeletedEventArgs : DiscordEventArgs
    {
        /// <summary>
        ///     Gets the id of the guild the invite's channel was in.
        ///     Returns <see langword="null"/>, if the invite was for a group channel.
        /// </summary>
        public Snowflake? GuildId { get; }

        /// <summary>
        ///     Gets the id of the channel the invite was for.
        /// </summary>
        public Snowflake ChannelId { get; }

        /// <summary>
        ///     Gets the unique code of the invite.
        /// </summary>
        public string Code { get; }

        internal InviteDeletedEventArgs(
            DiscordClientBase client,
            Snowflake? guildId,
            Snowflake channelId,
            string code) : base(client)
        {
            GuildId = guildId;
            ChannelId = channelId;
            Code = code;
        }
    }
}
