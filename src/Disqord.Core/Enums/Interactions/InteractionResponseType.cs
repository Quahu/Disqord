namespace Disqord;

/// <summary>
///     Represents the response type to an interaction.
/// </summary>
public enum InteractionResponseType
{
    Pong = 1,

    ChannelMessage = 4,

    DeferredChannelMessage = 5,

    DeferredMessageUpdate = 6,

    MessageUpdate = 7,

    ApplicationCommandAutoComplete = 8,

    Modal = 9
}