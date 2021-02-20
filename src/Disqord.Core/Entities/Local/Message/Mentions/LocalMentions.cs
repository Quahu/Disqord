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
        ///     Gets mentions in which all mentions are ignored.
        /// </summary>
        public static LocalMentions None => LocalMentionsBuilder.None.Build();

        /// <summary>
        ///     Gets mentions in which all <c>@everyone</c> mentions are ignored.
        /// </summary>
        public static LocalMentions ExceptEveryone => LocalMentionsBuilder.ExceptEveryone.Build();

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
