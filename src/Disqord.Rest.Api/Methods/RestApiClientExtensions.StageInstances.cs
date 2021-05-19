using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<StageInstanceJsonModel> CreateStageInstanceAsync(this IRestApiClient client, Snowflake channelId, CreateStageInstanceJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.StageInstances.CreateStageInstance, channelId);
            return client.ExecuteAsync<StageInstanceJsonModel>(route, content, options);
        }

        public static Task<StageInstanceJsonModel> FetchStageInstanceAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.StageInstances.FetchStageInstance, channelId);
            return client.ExecuteAsync<StageInstanceJsonModel>(route, null, options);
        }

        public static Task<StageInstanceJsonModel> ModifyStageInstanceAsync(this IRestApiClient client, Snowflake channelId, ModifyStageInstanceJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.StageInstances.ModifyStageInstance, channelId);
            return client.ExecuteAsync<StageInstanceJsonModel>(route, content, options);
        }

        public static Task DeleteStageInstanceAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.StageInstances.DeleteStageInstance, channelId);
            return client.ExecuteAsync(route, null, options);
        }
    }
}
