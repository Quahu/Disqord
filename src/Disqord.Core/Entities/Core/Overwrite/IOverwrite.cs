using Disqord.Models;

namespace Disqord;

/// <summary>
///     Represents a permission overwrite for a guild channel.
/// </summary>
public interface IOverwrite : IChannelEntity, IJsonUpdatable<OverwriteJsonModel>
{
    /// <summary>
    ///     Gets the target ID of this overwrite.
    /// </summary>
    Snowflake TargetId { get; }

    /// <summary>
    ///     Gets the <see cref="OverwriteTargetType"/> of this overwrite describing what the <see cref="TargetId"/> is of.
    /// </summary>
    OverwriteTargetType TargetType { get; }

    /// <summary>
    ///     Gets the overwritten permissions of this overwrite.
    /// </summary>
    OverwritePermissions Permissions { get; }
}