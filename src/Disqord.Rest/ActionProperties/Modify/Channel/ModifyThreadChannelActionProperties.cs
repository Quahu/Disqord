using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qommon;

namespace Disqord;

public class ModifyThreadChannelActionProperties : ModifyMessageGuildChannelActionProperties
{
    public Optional<bool> IsArchived { internal get; set; }

    public Optional<TimeSpan> AutomaticArchiveDuration { internal get; set; }

    public Optional<bool> IsLocked { internal get; set; }

    public Optional<bool> AllowsInvitation { internal get; set; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Optional<IEnumerable<LocalOverwrite>> Overwrites
    {
        internal get => Optional<IEnumerable<LocalOverwrite>>.Empty;
        set => throw new InvalidOperationException("Thread channels do not support modifying overwrites. Modify the parent channel's overwrites instead.");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Optional<int> Position
    {
        internal get => Optional<int>.Empty;
        set => throw new InvalidOperationException("Thread channels do not support modifying positions.");
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public override Optional<Snowflake?> CategoryId
    {
        internal get => Optional<Snowflake?>.Empty;
        set => throw new InvalidOperationException("Thread channels do not support modifying the category ID. Modify the parent channel's category ID instead.");
    }

    public Optional<IEnumerable<Snowflake>> TagIds { internal get; set; }

    internal ModifyThreadChannelActionProperties()
    { }
}
