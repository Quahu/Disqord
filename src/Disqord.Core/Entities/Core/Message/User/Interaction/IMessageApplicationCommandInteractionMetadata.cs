using System;

namespace Disqord;

/// <inheritdoc/>
public interface IMessageApplicationCommandInteractionMetadata : IMessageInteractionMetadata
{
    /// <summary>
    ///     Gets the name of the application command.
    /// </summary>
    /// <remarks>
    ///     Currently, as of September 14th 2025, this is not documented.
    ///     The property will throw an exception if the value is not available.
    /// </remarks>
    /// <exception cref="InvalidOperationException"> The property is not available. </exception>
    string CommandName { get; }

    /// <summary>
    ///     Gets the type of the application command.
    /// </summary>
    /// <remarks>
    ///     Currently, as of September 14th 2025, this is not documented.
    ///     The property will throw an exception if the value is not available.
    /// </remarks>
    /// <exception cref="InvalidOperationException"> The property is not available. </exception>
    ApplicationCommandType CommandType { get; }

    /// <summary>
    ///     Gets the target user the command was run on if the message originates from a <see cref="ApplicationCommandType.User"/> command;
    ///     gets <see langword="null"/> otherwise.
    /// </summary>
    IUser? TargetUser { get; }

    /// <summary>
    ///     Gets the ID of the target message the command was run on if the message originates from a <see cref="ApplicationCommandType.Message"/> command;
    ///     gets <see langword="null"/> otherwise.
    /// </summary>
    /// <remarks>
    ///      The original response message will also have <see cref="IUserMessage.Reference"/> and <see cref="IUserMessage.ReferencedMessage"/> pointing to this message.
    /// </remarks>
    Snowflake? TargetMessageId { get; }
}
