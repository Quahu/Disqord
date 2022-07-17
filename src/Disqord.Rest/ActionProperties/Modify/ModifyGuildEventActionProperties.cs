using System;
using System.IO;
using Qommon;

namespace Disqord.Rest;

public sealed class ModifyGuildEventActionProperties
{
    public Optional<Snowflake> ChannelId { internal get; set; }

    public Optional<string> Location { internal get; set; }

    public Optional<string> Name { internal get; set; }

    public Optional<PrivacyLevel> PrivacyLevel { internal get; set; }

    public Optional<DateTimeOffset> StartsAt { internal get; set; }

    public Optional<DateTimeOffset> EndsAt { internal get; set; }

    public Optional<string> Description { internal get; set; }

    public Optional<GuildEventTargetType> TargetType { internal get; set; }

    public Optional<GuildEventStatus> Status { internal get; set; }

    public Optional<Stream> CoverImage { internal get; set; }
}