using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord
{
    /// <summary>
    ///     Represents a guild template.
    /// </summary>
    public interface IGuildTemplate : IGuildEntity, INamable, IJsonUpdatable<GuildTemplateJsonModel>
    {
        /// <summary>
        ///     Gets the code of the template.
        /// </summary>
        string Code { get; }
        
        /// <summary>
        ///     Gets the description of the template.
        /// </summary>
        string Description { get; }
        
        /// <summary>
        ///     Gets the usage count of the template.
        /// </summary>
        int UsageCount { get; }
        
        /// <summary>
        ///     Gets the creator ID of the template.
        /// </summary>
        Snowflake CreatorId { get; }
        
        /// <summary>
        ///     Gets the creator of the template.
        /// </summary>
        IUser Creator { get; }
        
        /// <summary>
        ///     Gets when this template was created.
        /// </summary>
        DateTimeOffset CreatedAt { get; }
        
        /// <summary>
        ///     Gets when this template was updated.
        /// </summary>
        DateTimeOffset UpdatedAt { get; }
        
        /// <summary>
        ///     Gets the serialized source guild of the template.
        /// </summary>
        IJsonObject SerializedSourceGuild { get; }
        
        /// <summary>
        ///     Gets whether the template has unsynced changes.
        /// </summary>
        bool? IsDirty { get; }
    }
}