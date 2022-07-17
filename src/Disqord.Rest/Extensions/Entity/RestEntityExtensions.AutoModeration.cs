using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IAutoModerationRule> ModifyAsync(this IAutoModerationRule rule,
        Action<ModifyAutoModerationRuleActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = rule.GetRestClient();
        return client.ModifyAutoModerationRuleAsync(rule.GuildId, rule.Id, action, options, cancellationToken);
    }

    public static Task DeleteAsync(this IAutoModerationRule rule,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = rule.GetRestClient();
        return client.DeleteAutoModerationRuleAsync(rule.GuildId, rule.Id, options, cancellationToken);
    }
}