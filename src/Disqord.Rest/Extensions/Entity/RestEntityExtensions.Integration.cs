using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task DeleteAsync(this IIntegration integration, IRestRequestOptions options = null)
        {
            var client = integration.GetRestClient();
            return client.DeleteIntegrationAsync(integration.GuildId, integration.Id, options);
        }
    }
}
