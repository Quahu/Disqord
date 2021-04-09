using System;
using Disqord.Gateway;

namespace Disqord.Extensions.Interactivity.Menus
{
    public sealed class ButtonEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the user who triggered the button.
        /// </summary>
        public Snowflake UserId { get; }

        /// <summary>
        ///     Gets the message on which the button was triggered.
        ///     Returns <see langword="null"/> if the message was not cached.
        /// </summary>
        public CachedUserMessage Message { get; }

        /// <summary>
        ///     Gets the ID of the guild in which the button was triggered.
        ///     Returns <see langword="null"/> if it was triggered in a private channel.
        /// </summary>
        public Snowflake? GuildId { get; }

        /// <summary>
        ///     Gets the member who triggered the button.
        ///     Returns <see langword="null"/> if it was triggered in a private channel or if it the trigger was a removal.
        /// </summary>
        public IMember Member { get; }

        /// <summary>
        ///     Gets the emoji that was used that triggered the button.
        /// </summary>
        public IEmoji Emoji { get; }

        /// <summary>
        ///     Gets whether the button was triggered by adding a reaction as opposed to removing it.
        /// </summary>
        public bool WasAdded { get; }

        internal ButtonEventArgs(ReactionAddedEventArgs e)
        {
            UserId = e.UserId;
            Message = e.Message;
            GuildId = e.GuildId;
            Member = e.Member;
            Emoji = e.Emoji;
            WasAdded = true;
        }

        internal ButtonEventArgs(ReactionRemovedEventArgs e)
        {
            UserId = e.UserId;
            Message = e.Message;
            GuildId = e.GuildId;
            // Member isn't provided.
            Emoji = e.Emoji;
            WasAdded = false;
        }
    }
}
