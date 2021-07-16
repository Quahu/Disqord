using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<StageInstanceJsonModel> CreateStageAsync(this IRestApiClient client, Snowflake channelId, CreateStageInstanceJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Stages.CreateStage, channelId);
            return client.ExecuteAsync<StageInstanceJsonModel>(route, content, options);
        }

        public static Task<StageInstanceJsonModel> FetchStageAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Stages.FetchStage, channelId);
            return client.ExecuteAsync<StageInstanceJsonModel>(route, null, options);
        }

        public static Task<StageInstanceJsonModel> ModifyStageAsync(this IRestApiClient client, Snowflake channelId, ModifyStageInstanceJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Stages.ModifyStage, channelId);
            return client.ExecuteAsync<StageInstanceJsonModel>(route, content, options);
        }

        public static Task DeleteStageAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Stages.DeleteStage, channelId);
            return client.ExecuteAsync(route, null, options);
        }
    }
}
