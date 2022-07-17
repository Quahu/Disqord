using System;
using System.Linq;
using Qommon;

namespace Disqord.Rest.Api;

internal static partial class ActionPropertiesConversion
{
    public static ModifyAutoModerationRuleJsonRestRequestContent ToContent(this Action<ModifyAutoModerationRuleActionProperties> action)
    {
        Guard.IsNotNull(action);

        var properties = new ModifyAutoModerationRuleActionProperties();
        action(properties);

        return new ModifyAutoModerationRuleJsonRestRequestContent
        {
            Name = properties.Name,
            EventType = properties.EventType,
            TriggerMetadata = Optional.Convert(properties.TriggerMetadata, x => x.ToModel()),
            Actions = Optional.Convert(properties.Actions, x => x.Select(y => y.ToModel()).ToArray()),
            Enabled = properties.IsEnabled,
            ExemptRoles = Optional.Convert(properties.ExemptRoleIds, x => x.ToArray()),
            ExemptChannels = Optional.Convert(properties.ExemptChannelIds, x => x.ToArray())
        };
    }
}