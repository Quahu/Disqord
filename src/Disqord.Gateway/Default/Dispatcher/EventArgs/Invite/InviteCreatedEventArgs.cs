using System;

namespace Disqord.Gateway
{
    public class InviteCreatedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the optional ID of the guild in which the invite was created.
        /// </summary>
        public Optional<Snowflake> TargetGuildId { get; }

        /// <summary>
        ///     Gets the ID of the channel the invite was created for.
        /// </summary>
        public Snowflake TargetChannelId { get; }

        /// <summary>
        ///     Gets the code of the created invite.
        /// </summary>
        public string Code { get; }

        /// <summary>
        ///     Gets when the invite was created.
        /// </summary>
        public DateTimeOffset CreatedAt { get; }

        /// <summary>
        ///     Gets the optional user who created the invite.
        /// </summary>
        public IUser Inviter { get; }

        /// <summary>
        ///     Gets the maximum age of the invite.
        /// </summary>
        public int MaxAge { get; }

        /// <summary>
        ///     Gets the maximum uses of the invite.
        /// </summary>
        public int MaxUses { get; }

        /// <summary>
        ///     Gets the optional target type of the invite.
        ///     Returns <see langword="null"/> when the invite's channel is not a voice channel.
        /// </summary>
        public InviteTargetType? TargetType { get; }

        /// <summary>
        ///     Gets the optional target user of the invite.
        ///     Returns <see langword="null"/> when <see cref="IGuildInvite.TargetType"/> is not <see cref="InviteTargetType.Stream"/>.
        /// </summary>
        public IUser TargetUser { get; }

        /// <summary>
        ///     Gets the optional target application of the invite.
        ///     Returns <see langword="null"/> when <see cref="IGuildInvite.TargetType"/> is not <see cref="InviteTargetType.EmbeddedApplication"/>.
        /// </summary>
        public IApplication TargetApplication { get; }

        /// <summary>
        ///     Gets whether the invite is temporary membership only.
        /// </summary>
        public bool IsTemporaryMembership { get; }

        /// <summary>
        ///     Gets the current uses of the invite.
        /// </summary>
        public int Uses { get; }

        public InviteCreatedEventArgs(
            Optional<Snowflake> targetGuildId,
            Snowflake targetChannelId,
            string code,
            DateTimeOffset createdAt,
            IUser inviter,
            int maxAge,
            int maxUses,
            InviteTargetType? targetType,
            IUser targetUser,
            IApplication targetApplication,
            bool isTemporaryMembership,
            int uses)
        {
            TargetGuildId = targetGuildId;
            TargetChannelId = targetChannelId;
            Code = code;
            CreatedAt = createdAt;
            Inviter = inviter;
            MaxAge = maxAge;
            MaxUses = maxUses;
            TargetType = targetType;
            TargetUser = targetUser;
            TargetApplication = targetApplication;
            IsTemporaryMembership = isTemporaryMembership;
            Uses = uses;
        }
    }
}
