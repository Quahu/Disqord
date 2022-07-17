using System.Collections.Generic;
using Qommon;

namespace Disqord.Rest;

public sealed class ModifyAutoModerationRuleActionProperties
{
    public Optional<string> Name { internal get; set; }

    public Optional<AutoModerationEventType> EventType;

    public Optional<LocalAutoModerationTriggerMetadata> TriggerMetadata;

    public Optional<IEnumerable<LocalAutoModerationAction>> Actions;

    public Optional<bool> IsEnabled;

    public Optional<IEnumerable<Snowflake>> ExemptRoleIds;

    public Optional<IEnumerable<Snowflake>> ExemptChannelIds;
}