using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an application of a guild integration.
/// </summary>
public interface IIntegrationApplication : ISnowflakeEntity, INamableEntity, IJsonUpdatable<IntegrationApplicationJsonModel>
{
    /// <summary>
    ///     Gets the icon hash of this application.
    /// </summary>
    string? IconHash { get; }

    /// <summary>
    ///     Gets the description of this application.
    /// </summary>
    string Description { get; }

    /// <summary>
    ///     Gets the summary of this application.
    /// </summary>
    string? Summary { get; }

    /// <summary>
    ///     Gets the bot user of this application.
    /// </summary>
    /// <returns>
    ///     <see langword="null"/> for non-bot applications.
    /// </returns>
    IUser? Bot { get; }
}
