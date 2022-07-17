using System.ComponentModel;
using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents an interaction.
/// </summary>
public interface IInteraction : ISnowflakeEntity, IJsonUpdatable<InteractionJsonModel>
{
    /// <summary>
    ///     Gets the time at which this interaction was received locally.
    /// </summary>
    /// <remarks>
    ///     This is an internal Disqord API and should not be used.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    long __ReceivedAt { get; }

    /// <summary>
    ///     Gets the ID of the application of this interaction.
    /// </summary>
    Snowflake ApplicationId { get; }

    /// <summary>
    ///     Gets the version of this interaction.
    /// </summary>
    int Version { get; }

    /// <summary>
    ///     Gets the type of this interaction.
    /// </summary>
    InteractionType Type { get; }

    /// <summary>
    ///     Gets the token of this interaction.
    /// </summary>
    string Token { get; }
}