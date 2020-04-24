using System;

namespace Disqord.Events
{
    public sealed class InviteCreatedEventArgs : DiscordEventArgs
    {
        public CachedGuild Guild { get; }

        public SnowflakeOptional<CachedChannel> Channel { get; }

        /// <summary>
        ///     Gets the user that created the invite. Returns <see langword="null"/>, if the invite was created by, for example, a widget.
        /// </summary>
        public CachedUser Inviter { get; }

        public string Code { get; }

        public bool IsTemporary { get; }

        public int MaxUses { get; }

        public int MaxAge { get; }

        public DateTimeOffset CreatedAt { get; }

        internal InviteCreatedEventArgs(
            DiscordClientBase client,
            CachedGuild guild,
            SnowflakeOptional<CachedChannel> channel,
            CachedUser inviter,
            string code,
            bool isTemporary,
            int maxUses,
            int maxAge,
            DateTimeOffset createdAt) : base(client)
        {
            Guild = guild;
            Channel = channel;
            Inviter = inviter;
            Code = code;
            IsTemporary = isTemporary;
            MaxUses = maxUses;
            MaxAge = maxAge;
            CreatedAt = createdAt;
        }
    }
}
