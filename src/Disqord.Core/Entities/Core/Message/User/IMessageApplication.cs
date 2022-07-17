namespace Disqord;

/// <summary>
///     Represents an application embedded within a message.
/// </summary>
public interface IMessageApplication : IIdentifiableEntity, INamableEntity
{
    /// <summary>
    ///     Gets the cover image hash of this application.
    /// </summary>
    string? CoverImageHash { get; }

    /// <summary>
    ///     Gets the description of this application.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the icon image hash of this application.
    /// </summary>
    string? IconHash { get; }
}
