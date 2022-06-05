using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IAutoModerationRule>> FetchAutoModerationRulesAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchAutoModerationRulesAsync(guildId, options, cancellationToken);
            return models.ToReadOnlyList(client, static (x, client) => new TransientAutoModerationRule(client, x));
        }

        public static async Task<IAutoModerationRule> FetchAutoModerationRuleAsync(this IRestClient client,
            Snowflake guildId, Snowflake ruleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchAutoModerationRuleAsync(guildId, ruleId, options, cancellationToken);
            return new TransientAutoModerationRule(client, model);
        }

        public static Task DeleteAutoModerationRuleAsync(this IRestClient client,
            Snowflake guildId, Snowflake ruleId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
            => client.ApiClient.DeleteAutoModerationRuleAsync(guildId, ruleId, options, cancellationToken);
    }
}
