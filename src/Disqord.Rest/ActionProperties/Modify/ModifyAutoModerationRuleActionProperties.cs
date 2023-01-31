using System.Collections.Generic;
using Qommon;

namespace Disqord.Rest;

public sealed class ModifyAutoModerationRuleActionProperties
{
    public Optional<string> Name { internal get; set; }

    public Optional<AutoModerationEventType> EventType { internal get; set; }

    public Optional<LocalAutoModerationTriggerMetadata> TriggerMetadata { internal get; set; }

    public Optional<IEnumerable<LocalAutoModerationAction>> Actions { internal get; set; }

    public Optional<bool> IsEnabled { internal get; set; }

    public Optional<IEnumerable<Snowflake>> ExemptRoleIds { internal get; set; }

    public Optional<IEnumerable<Snowflake>> ExemptChannelIds { internal get; set; }
}
