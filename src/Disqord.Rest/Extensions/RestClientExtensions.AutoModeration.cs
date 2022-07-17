using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IReadOnlyList<IAutoModerationRule>> FetchAutoModerationRulesAsync(this IRestClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchAutoModerationRulesAsync(guildId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(client, static (x, client) => new TransientAutoModerationRule(client, x));
    }

    public static async Task<IAutoModerationRule> FetchAutoModerationRuleAsync(this IRestClient client,
        Snowflake guildId, Snowflake ruleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var model = await client.ApiClient.FetchAutoModerationRuleAsync(guildId, ruleId, options, cancellationToken).ConfigureAwait(false);
        return new TransientAutoModerationRule(client, model);
    }

    public static async Task<IAutoModerationRule> CreateAutoModerationRuleAsync(this IRestClient client,
        Snowflake guildId,
        string name, AutoModerationEventType eventType, AutoModerationRuleTrigger trigger,
        IEnumerable<LocalAutoModerationAction> actions, Action<CreateAutoModerationRuleActionProperties>? action = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var properties = new CreateAutoModerationRuleActionProperties();
        action?.Invoke(properties);

        var content = new CreateAutoModerationRuleJsonRestRequestContent
        {
            Name = name,
            EventType = eventType,
            Trigger = trigger,
            Actions = actions.Select(x => x.ToModel()).ToArray(),
            TriggerMetadata = Optional.Convert(properties.TriggerMetadata, x => x.ToModel()),
            Enabled = properties.IsEnabled,
            ExemptRoles = Optional.Convert(properties.ExemptRoleIds, x => x.ToArray()),
            ExemptChannels = Optional.Convert(properties.ExemptChannelIds, x => x.ToArray())
        };

        var model = await client.ApiClient.CreateAutoModerationRuleAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientAutoModerationRule(client, model);
    }

    public static async Task<IAutoModerationRule> ModifyAutoModerationRuleAsync(this IRestClient client,
        Snowflake guildId, Snowflake ruleId,
        Action<ModifyAutoModerationRuleActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var content = action.ToContent();
        var model = await client.ApiClient.ModifyAutoModerationRuleAsync(guildId, ruleId, content, options, cancellationToken).ConfigureAwait(false);
        return new TransientAutoModerationRule(client, model);
    }

    public static Task DeleteAutoModerationRuleAsync(this IRestClient client,
        Snowflake guildId, Snowflake ruleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.ApiClient.DeleteAutoModerationRuleAsync(guildId, ruleId, options, cancellationToken);
    }
}