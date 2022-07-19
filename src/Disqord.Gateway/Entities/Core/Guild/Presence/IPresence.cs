using System.Collections.Generic;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway;

/// <summary>
///     Represents the presence of a member, i.e. their status and activities.
/// </summary>
public interface IPresence : IGatewayEntity, IGuildEntity, IJsonUpdatable<PresenceJsonModel>
{
    /// <summary>
    ///     Gets the ID of the member this presence is for.
    /// </summary>
    Snowflake MemberId { get; }

    /// <summary>
    ///     Gets the activities of this presence.
    /// </summary>
    /// <example>
    ///     Checking if the member is listening to Spotify and to what track.
    ///     <code>
    ///     var spotifyActivity = presence.Activities.FirstOrDefault(x => x is ISpotifyActivity) as ISpotifyActivity;
    ///     if (spotifyActivity != null)
    ///     {
    ///         var track = spotifyActivity.TrackTitle;
    ///     }
    ///     </code>
    /// </example>
    IReadOnlyList<IActivity> Activities { get; }

    /// <summary>
    ///     Gets the <see cref="UserStatus"/> of this presence.
    /// </summary>
    /// <remarks>
    ///     This will be the status shown in the desktop client.
    /// </remarks>
    UserStatus Status { get; }

    /// <summary>
    ///     Gets the <see cref="UserClient"/>s and their respective <see cref="UserStatus"/>es of this presence.
    /// </summary>
    /// <example>
    ///     Checking if the member is on a mobile device.
    ///     <code>
    ///     var isMobile = presence.Statuses.ContainsKey(UserClient.Mobile);
    ///     </code>
    /// </example>
    IReadOnlyDictionary<UserClient, UserStatus> Statuses { get; }
}
