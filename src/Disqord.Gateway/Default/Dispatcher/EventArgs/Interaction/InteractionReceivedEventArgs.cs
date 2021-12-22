using System;
using System.Globalization;

namespace Disqord.Gateway
{
    public class InteractionReceivedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the interaction was received.
        ///     Returns <see langword="null"/> if the interaction was received in a private channel.
        /// </summary>
        public Snowflake? GuildId => Interaction.GuildId;

        /// <summary>
        ///     Gets the ID of the channel in which the interaction was received in.
        /// </summary>
        public Snowflake ChannelId => Interaction.ChannelId;

        /// <summary>
        ///     Gets the ID of the received interaction.
        /// </summary>
        public Snowflake InteractionId => Interaction.Id;

        /// <summary>
        ///     Gets the received interaction.
        /// </summary>
        public virtual IInteraction Interaction { get; }

        /// <summary>
        ///     Gets the cached member that triggered this interaction.
        ///     Returns <see langword="null"/> if the member was not cached or if the interaction was triggered outside of a guild.
        /// </summary>
        /// <remarks>
        ///     If this returns <see langword="null"/>, retrieve the author from the <see cref="Interaction"/> instead.
        /// </remarks>
        public CachedMember Member { get; }

        /// <summary>
        ///     Gets the ID of the interaction author.
        /// </summary>
        public Snowflake AuthorId => Interaction.Author.Id;

        /// <summary>
        ///     Gets the locale the interaction author.
        ///     Returns <see langword="null"/> if the type of the received interaction is <see cref="InteractionType.Ping"/>.
        /// </summary>
        public CultureInfo Locale => Interaction.Locale;

        /// <summary>
        ///     Gets the preferred locale of the guild in which the interaction was received.
        ///     Returns <see langword="null"/> if the interaction was received in a private channel.
        /// </summary>
        public CultureInfo GuildPreferredLocale => Interaction.GuildPreferredLocale;

        public InteractionReceivedEventArgs(
            IInteraction interaction,
            CachedMember member)
        {
            Interaction = interaction;
            Member = member;
        }
    }
}
