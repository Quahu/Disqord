using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task LeaveAsync(this IGuild guild, IRestRequestOptions options = null)
        {
            var client = guild.GetRestClient();
            return client.LeaveGuildAsync(guild.Id, options);
        }

        public static Task<IDirectChannel> CreateDirectChannelAsync(this IUser user, IRestRequestOptions options = null)
        {
            var client = user.GetRestClient();
            return client.CreateDirectChannelAsync(user.Id, options);
        }
    }
}
