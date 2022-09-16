using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class CreateThreadChannelActionProperties
{
    public Optional<TimeSpan> AutomaticArchiveDuration { internal get; set; }

    public Optional<TimeSpan> Slowmode { internal get; set; }

    public Optional<IEnumerable<Snowflake>> TagIds { internal get; set; }

    internal CreateThreadChannelActionProperties()
    { }
}
