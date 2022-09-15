using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class ModifyForumChannelActionProperties : ModifyMessageGuildChannelActionProperties
{
    public Optional<string> Topic { internal get; set; }

    public Optional<bool> IsAgeRestricted { internal get; set; }

    public Optional<TimeSpan> DefaultAutomaticArchiveDuration { internal get; set; }

    public Optional<IEnumerable<LocalForumTag>> Tags { internal get; set; }

    public Optional<LocalEmoji?> DefaultReactionEmoji { internal get; set; }

    public Optional<TimeSpan> DefaultThreadSlowmode { internal get; set; }

    internal ModifyForumChannelActionProperties()
    { }
}
