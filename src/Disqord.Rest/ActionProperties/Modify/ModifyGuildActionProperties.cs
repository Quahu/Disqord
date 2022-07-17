using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Qommon;

namespace Disqord;

public sealed class ModifyGuildActionProperties
{
    public Optional<string> Name { internal get; set; }

    public Optional<GuildVerificationLevel> VerificationLevel { internal get; set; }

    public Optional<GuildNotificationLevel> NotificationLevel { internal get; set; }

    public Optional<GuildContentFilterLevel> ContentFilterLevel { internal get; set; }

    public Optional<Snowflake?> AfkChannelId { internal get; set; }

    public Optional<int> AfkTimeout { internal get; set; }

    public Optional<Stream> Icon { internal get; set; }

    public Optional<Snowflake> OwnerId { internal get; set; }

    public Optional<Stream> Splash { internal get; set; }

    public Optional<Stream> DiscoverySplash { internal get; set; }

    public Optional<Stream> Banner { internal get; set; }

    public Optional<Snowflake?> SystemChannelId { internal get; set; }

    public Optional<SystemChannelFlags> SystemChannelFlags { internal get; set; }

    public Optional<Snowflake?> RulesChannelId { internal get; set; }

    public Optional<Snowflake?> PublicUpdatesChannelId { internal get; set; }

    public Optional<CultureInfo> PreferredLocale { internal get; set; }

    public Optional<IEnumerable<string>> Features { internal get; set; }

    public Optional<string> Description { internal get; set; }

    public Optional<bool> IsBoostProgressBarEnabled { internal get; set; }

    internal ModifyGuildActionProperties()
    { }
}