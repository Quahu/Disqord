using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<ICurrentUser> ModifyAsync(this ICurrentUser user, Action<ModifyCurrentUserActionProperties> action, IRestRequestOptions options = null)
        {
            var client = user.GetRestClient();
            return client.ModifyCurrentUserAsync(action);
        }

        public static Task<IDirectChannel> CreateDirectChannelAsync(this IUser user, IRestRequestOptions options = null)
        {
            var client = user.GetRestClient();
            return client.CreateDirectChannelAsync(user.Id, options);
        }

        public static async Task<IUserMessage> SendMessageAsync(this IUser user, LocalMessage message, IRestRequestOptions options = null)
        {
            var channel = await user.CreateDirectChannelAsync(options).ConfigureAwait(false);
            return await channel.SendMessageAsync(message).ConfigureAwait(false);
        }
    }
}
