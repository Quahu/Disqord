using System.Collections.Generic;
using Qommon;

namespace Disqord;

public sealed class CreateAutoModerationRuleActionProperties
{
    public Optional<LocalAutoModerationTriggerMetadata> TriggerMetadata { internal get; set; }

    public Optional<bool> IsEnabled { internal get; set; }

    public Optional<IEnumerable<Snowflake>> ExemptRoleIds { internal get; set; }

    public Optional<IEnumerable<Snowflake>> ExemptChannelIds { internal get; set; }
}