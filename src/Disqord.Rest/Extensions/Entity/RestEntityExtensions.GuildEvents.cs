using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IGuildEvent> ModifyAsync(this IGuildEvent guildEvent, Action<ModifyGuildEventActionProperties> action, IRestRequestOptions options = null)
        {
            var client = guildEvent.GetRestClient();
            return client.ModifyGuildEventAsync(guildEvent.Id, action, options);
        }

        public static Task DeleteAsync(this IGuildEvent guildEvent, IRestRequestOptions options = null)
        {
            var client = guildEvent.GetRestClient();
            return client.DeleteGuildEventAsync(guildEvent.Id, options);
        }
    }
}
