using System;
using System.Collections.Generic;

namespace Disqord.Gateway;

/// <summary>
///     Represents a member's Spotify activity,
///     i.e. a member listening to a track on Spotify.
/// </summary>
public interface ISpotifyActivity : IActivity
{
    string INamableEntity.Name => "Spotify";

    /// <summary>
    ///     Gets the ID of the party of this activity.
    /// </summary>
    string? PartyId { get; }

    /// <summary>
    ///     Gets the title of the track of this activity.
    /// </summary>
    string? TrackTitle { get; }

    /// <summary>
    ///     Gets the ID of the track of this activity.
    /// </summary>
    string? TrackId { get; }

    /// <summary>
    ///     Gets the album title of this activity.
    /// </summary>
    string? AlbumTitle { get; }

    /// <summary>
    ///     Gets the ID of the album of this activity.
    /// </summary>
    string? AlbumId { get; }

    /// <summary>
    ///     Gets the artists of the track of this activity.
    /// </summary>
    IReadOnlyList<string> Artists { get; }

    /// <summary>
    ///     Gets when the track of this activity started.
    /// </summary>
    DateTimeOffset StartedAt { get; }

    /// <summary>
    ///     Gets when the track of activity ends.
    /// </summary>
    DateTimeOffset EndsAt { get; }
}
