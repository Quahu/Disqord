using System;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway;

public class MessageUpdatedEventArgs : EventArgs
{
    /// <summary>
    ///     Gets the ID of the guild in which the update occurred.
    ///     Returns <see langword="null"/> if the message updated in a private channel.
    /// </summary>
    public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

    /// <summary>
    ///     Gets the ID of the channel in which the update occurred.
    /// </summary>
    public Snowflake ChannelId => Model.ChannelId;

    /// <summary>
    ///     Gets the ID of the message that was updated.
    /// </summary>
    public Snowflake MessageId => Model.Id;

    /// <summary>
    ///     Gets the message in the state before the update occurred.
    ///     Returns <see langword="null"/> if the message was not cached.
    /// </summary>
    public CachedUserMessage? OldMessage { get; }

    /// <summary>
    ///     Gets the updated message.
    ///     Returns <see langword="null"/>, if the message was not cached.
    /// </summary>
    public CachedUserMessage? NewMessage { get; }

    /// <summary>
    ///     Gets the payload model with the update data.
    /// </summary>
    /// <remarks>
    ///     This is exposed because the model often contains little to no data required to construct a proper message object.
    ///     Inspect this object to detect changes for uncached messages.
    /// </remarks>
    public MessageUpdateJsonModel Model { get; }

    public MessageUpdatedEventArgs(
        CachedUserMessage? oldMessage,
        CachedUserMessage? newMessage,
        MessageUpdateJsonModel model)
    {
        OldMessage = oldMessage;
        NewMessage = newMessage;
        Model = model;
    }
}
