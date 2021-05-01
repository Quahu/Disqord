using System;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a member's activity.
    /// </summary>
    public interface IActivity : IEntity, IJsonUpdatable<ActivityJsonModel>
    {
        /// <summary>
        ///     Gets the name of this activity.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Gets the type of this activity.
        /// </summary>
        ActivityType Type { get; }
        
        /// <summary>
        ///     Gets the creation date of this activity.
        /// </summary>
        DateTimeOffset CreatedAt { get; }
    }
}
