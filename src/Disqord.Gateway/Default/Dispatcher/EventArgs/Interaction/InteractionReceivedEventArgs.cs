using System;

namespace Disqord.Gateway;

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
    public virtual IUserInteraction Interaction { get; }

    /// <summary>
    ///     Gets the cached member that triggered this interaction.
    ///     Returns <see langword="null"/> if the member was not cached or if the interaction was triggered outside of a guild.
    /// </summary>
    /// <remarks>
    ///     If this returns <see langword="null"/>, retrieve the author from the <see cref="Interaction"/> instead.
    /// </remarks>
    public CachedMember? Member { get; }

    /// <summary>
    ///     Gets the ID of the interaction author.
    /// </summary>
    public Snowflake AuthorId => Interaction.Author.Id;

    public InteractionReceivedEventArgs(
        IUserInteraction interaction,
        CachedMember? member)
    {
        Interaction = interaction;
        Member = member;
    }
}
