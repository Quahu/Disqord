using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

/// <inheritdoc cref="IAutoModerationActionMetadata"/>
public class TransientAutoModerationActionMetadata : TransientEntity<AutoModerationActionMetadataJsonModel>, IAutoModerationActionMetadata
{
    /// <inheritdoc/>
    public Snowflake? ChannelId => Model.ChannelId.GetValueOrNullable();

    /// <inheritdoc/>
    public TimeSpan? TimeoutDuration => Optional.ConvertOrDefault(Model.DurationSeconds, x => TimeSpan.FromSeconds(x));

    public TransientAutoModerationActionMetadata(AutoModerationActionMetadataJsonModel model)
        : base(model)
    { }
}