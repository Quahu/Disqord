using System;
using Qommon;

namespace Disqord;

public class ModifyForumChannelActionProperties : ModifyNestableChannelActionProperties
{
    public Optional<string> Topic { internal get; set; }

    public Optional<TimeSpan> Slowmode { internal get; set; }

    public Optional<bool> IsAgeRestricted { internal get; set; }

    public Optional<TimeSpan> DefaultAutomaticArchiveDuration { internal get; set; }

    internal ModifyForumChannelActionProperties()
    { }
}