using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord;

public class ModifyMediaChannelActionProperties : ModifyMessageGuildChannelActionProperties
{
    public Optional<string> Topic { internal get; set; }

    public Optional<bool> IsAgeRestricted { internal get; set; }

    public Optional<TimeSpan> DefaultAutomaticArchiveDuration { internal get; set; }

    public Optional<IEnumerable<LocalForumTag>> Tags { internal get; set; }

    public Optional<LocalEmoji?> DefaultReactionEmoji { internal get; set; }

    public Optional<TimeSpan> DefaultThreadSlowmode { internal get; set; }

    public Optional<ForumSortOrder?> DefaultSortOrder { internal get; set; }

    internal ModifyMediaChannelActionProperties()
    { }
}
