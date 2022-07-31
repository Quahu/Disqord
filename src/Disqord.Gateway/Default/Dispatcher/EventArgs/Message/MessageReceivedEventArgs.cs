using System;

namespace Disqord.Gateway;

public class MessageReceivedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the message was received.
    /// </summary>
    /// <returns>
    ///     The ID of the guild or <see langword="null"/> if the message was received in a private channel.
    /// </returns>
    public Snowflake? GuildId => Message.GuildId;

    /// <summary>
    ///     Gets the ID of the channel in which the message was received.
    /// </summary>
    public Snowflake ChannelId => Message.ChannelId;

    /// <summary>
    ///     Gets the ID of the message that was received.
    /// </summary>
    public Snowflake MessageId => Message.Id;

    /// <summary>
    ///     Gets the ID of the message author.
    /// </summary>
    public Snowflake AuthorId => Message.Author.Id;

    /// <summary>
    ///     Gets the message received.
    /// </summary>
    public IGatewayMessage Message { get; }

    /// <summary>
    ///     Gets the cached channel in which the message was received.
    /// </summary>
    /// <returns>
    ///     The channel or <see langword="null"/> if the channel was not cached of if the message was received outside of a guild.
    /// </returns>
    public CachedMessageGuildChannel? Channel { get; }

    /// <summary>
    ///     Gets the cached member that sent the message.
    /// </summary>
    /// <remarks>
    ///     If this returns <see langword="null"/>, retrieve the author from the <see cref="Message"/> instead.
    /// </remarks>
    /// <returns>
    ///     The member or <see langword="null"/> if the member was not cached or if the message was received outside of a guild.
    /// </returns>
    public CachedMember? Member { get; }

    public MessageReceivedEventArgs(
        IGatewayMessage message,
        CachedMessageGuildChannel? channel,
        CachedMember? member)
    {
        Message = message;
        Channel = channel;
        Member = member;
    }
}
