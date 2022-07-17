using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<AutoModerationRuleJsonModel[]> FetchAutoModerationRulesAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.AutoModeration.GetRules, guildId);
        return client.ExecuteAsync<AutoModerationRuleJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<AutoModerationRuleJsonModel> FetchAutoModerationRuleAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake ruleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.AutoModeration.GetRule, guildId, ruleId);
        return client.ExecuteAsync<AutoModerationRuleJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<AutoModerationRuleJsonModel> CreateAutoModerationRuleAsync(this IRestApiClient client,
        Snowflake guildId, CreateAutoModerationRuleJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.AutoModeration.CreateRule, guildId);
        return client.ExecuteAsync<AutoModerationRuleJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<AutoModerationRuleJsonModel> ModifyAutoModerationRuleAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake ruleId, ModifyAutoModerationRuleJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.AutoModeration.ModifyRule, guildId, ruleId);
        return client.ExecuteAsync<AutoModerationRuleJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteAutoModerationRuleAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake ruleId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.AutoModeration.DeleteRule, guildId, ruleId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }
}