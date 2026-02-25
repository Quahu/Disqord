using System.Collections.Generic;

namespace Disqord;

/// <summary>
///     Represents interaction information of a message.
/// </summary>
public interface IMessageInteractionMetadata : IIdentifiableEntity
{
    /// <summary>
    ///     Gets the interaction type of the message.
    /// </summary>
    InteractionType Type { get; }

    /// <summary>
    ///     Gets the author of the interaction.
    /// </summary>
    IUser Author { get; }

    /// <summary>
    ///     Gets the IDs for installation context(s) related to an interaction.
    ///     See <a href="https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-authorizing-integration-owners-object">Discord documentation.</a>
    /// </summary>
    IReadOnlyDictionary<ApplicationIntegrationType, Snowflake> AuthorizingIntegrationOwnerIds { get; }

    /// <summary>
    ///     Gets the ID of the original response message if the message is a follow-up message;
    ///     gets <see langword="null"/> otherwise.
    /// </summary>
    Snowflake? OriginalResponseMessageId { get; }
}
