using System;
using System.Collections.Generic;
using Qommon;

namespace Disqord;

public sealed class CreateForumChannelActionProperties : CreateNestedChannelActionProperties
{
    public Optional<string> Topic { internal get; set; }

    public Optional<TimeSpan> Slowmode { internal get; set; }

    public Optional<bool> IsAgeRestricted { internal get; set; }

    public Optional<TimeSpan> DefaultAutomaticArchiveDuration { internal get; set; }

    public Optional<IEnumerable<LocalForumTag>> Tags { internal get; set; }

    public Optional<LocalEmoji> DefaultReactionEmoji { internal get; set; }

    public Optional<TimeSpan> DefaultThreadSlowmode { internal get; set; }

    /// <summary>
    ///     Sets whether the forum channel requires a tag to be specified for threads created in it.
    /// </summary>
    /// <remarks>
    ///     This is a shorthand for applying <see cref="GuildChannelFlags.RequiresTag"/> to <see cref="CreateGuildChannelActionProperties.Flags"/>.
    /// </remarks>
    public bool RequiresTag
    {
        set
        {
            if (value)
            {
                Flags = Flags.GetValueOrDefault() | GuildChannelFlags.RequiresTag;
            }
            else
            {
                if (!Flags.HasValue)
                    return;

                Flags = Flags.Value & ~GuildChannelFlags.RequiresTag;
            }
        }
    }

    internal CreateForumChannelActionProperties()
    { }
}
