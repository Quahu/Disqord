using System.Collections.Generic;
using Disqord.Collections;

namespace Disqord
{
    /// <summary>
    ///     Represents mentions for a Discord message.
    /// </summary>
    public sealed class LocalMentions
    {
        /// <summary>
        ///     All mentions in the message's content are ignored.
        /// </summary>
        public static readonly LocalMentions None = new LocalMentionsBuilder()
            .WithParsedMentions(ParsedMention.None)
            .Build();

        /// <summary>
        ///     All <c>@everyone</c> mentions in the message's content are ignored.
        /// </summary>
        public static readonly LocalMentions ExceptEveryone = new LocalMentionsBuilder()
            .WithParsedMentions(ParsedMention.Users | ParsedMention.Roles)
            .Build();

        /// <summary>
        ///     Gets the mention types Discord will parse from the message's content.
        /// </summary>
        public ParsedMention ParsedMentions { get; }

        /// <summary>
        ///     Gets the user IDs that are going to be mentioned.
        /// </summary>
        public IReadOnlyList<Snowflake> UserIds { get; }

        /// <summary>
        ///     Gets the role IDs that are going to be mentioned.
        /// </summary>
        public IReadOnlyList<Snowflake> RoleIds { get; }

        internal LocalMentions(LocalMentionsBuilder builder)
        {
            ParsedMentions = builder.ParsedMentions;
            UserIds = builder.UserIds.ToReadOnlyList();
            RoleIds = builder.RoleIds.ToReadOnlyList();
        }
    }
}
