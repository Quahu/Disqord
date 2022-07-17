using System;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway;

/// <summary>
///     Represents a member's activity.
/// </summary>
public interface IActivity : IGatewayEntity, INamableEntity, IJsonUpdatable<ActivityJsonModel>
{
    /// <summary>
    ///     Gets the type of this activity.
    /// </summary>
    ActivityType Type { get; }

    /// <summary>
    ///     Gets the creation date of this activity.
    /// </summary>
    DateTimeOffset CreatedAt { get; }
}
