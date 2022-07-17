using System;
using Qommon;

namespace Disqord;

public class CreateThreadChannelActionProperties
{
    public Optional<TimeSpan> AutomaticArchiveDuration { internal get; set; }

    public Optional<TimeSpan> Slowmode { internal get; set; }

    internal CreateThreadChannelActionProperties()
    { }
}
