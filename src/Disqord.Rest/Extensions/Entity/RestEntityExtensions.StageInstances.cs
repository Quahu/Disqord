using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestEntityExtensions
    {
        public static Task<IStageInstance> ModifyAsync(this IStageInstance instance, Action<ModifyStageInstanceActionProperties> action, IRestRequestOptions options = null)
        {
            var client = instance.GetRestClient();
            return client.ModifyStageInstanceAsync(instance.ChannelId, action, options);
        }

        public static Task DeleteAsync(this IStageInstance instance, IRestRequestOptions options = null)
        {
            var client = instance.GetRestClient();
            return client.DeleteStageInstanceAsync(instance.ChannelId, options);
        }
    }
}