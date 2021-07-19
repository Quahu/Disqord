using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestEntityExtensions
    {
        public static Task<IStage> ModifyAsync(this IStage instance, Action<ModifyStageActionProperties> action, IRestRequestOptions options = null)
        {
            var client = instance.GetRestClient();
            return client.ModifyStageAsync(instance.ChannelId, action, options);
        }

        public static Task DeleteAsync(this IStage instance, IRestRequestOptions options = null)
        {
            var client = instance.GetRestClient();
            return client.DeleteStageAsync(instance.ChannelId, options);
        }
    }
}