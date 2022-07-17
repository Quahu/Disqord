namespace Disqord;

/// <summary>
///     Represents an activity embedded within a message.
/// </summary>
public interface IMessageActivity : IEntity
{
    /// <summary>
    ///     Gets the type of this activity.
    /// </summary>
    MessageActivityType Type { get; }

    /// <summary>
    ///     Gets the ID of the party of this activity.
    /// </summary>
    string? PartyId { get; }
}
