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

        public DateTimeOffset CreatedAt { get; }

        public IUser Inviter { get; }

        public int MaxAge { get; }

        public int MaxUses { get; }

        public InviteTargetType? TargetType { get; }

        public IUser TargetUser { get; }

        public IApplication TargetApplication { get; }

        public bool IsTemporary { get; }

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
            bool isTemporary,
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
            IsTemporary = isTemporary;
            Uses = uses;
        }
    }
}
