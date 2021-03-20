using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IDirectChannel> CreateDirectChannelAsync(this IUser user, IRestRequestOptions options = null)
        {
            var client = user.GetRestClient();
            return client.CreateDirectChannelAsync(user.Id, options);
        }
    }
}
