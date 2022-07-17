using System;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord;

/// <summary>
///     Represents a guild template.
/// </summary>
public interface IGuildTemplate : IGuildEntity, IClientEntity, INamableEntity, IJsonUpdatable<GuildTemplateJsonModel>
{
    /// <summary>
    ///     Gets the code of this template.
    /// </summary>
    string Code { get; }

    /// <summary>
    ///     Gets the description of this template.
    /// </summary>
    string? Description { get; }

    /// <summary>
    ///     Gets the uses of this template.
    /// </summary>
    int Uses { get; }

    /// <summary>
    ///     Gets the ID of the creator of this template.
    /// </summary>
    Snowflake CreatorId { get; }

    /// <summary>
    ///     Gets the creator of this template.
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
    ///     Gets the serialized guild of this template.
    /// </summary>
    IJsonObject SerializedGuild { get; }

    /// <summary>
    ///     Gets whether this template has unsynced changes.
    /// </summary>
    bool IsDirty { get; }
}
