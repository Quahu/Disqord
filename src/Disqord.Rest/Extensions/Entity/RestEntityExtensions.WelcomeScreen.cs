using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IGuildWelcomeScreen> ModifyAsync(this IGuildWelcomeScreen welcomeScreen, Action<ModifyWelcomeScreenActionProperties> action, IRestRequestOptions options = null)
        {
            var client = welcomeScreen.GetRestClient();
            return client.ModifyGuildWelcomeScreenAsync(welcomeScreen.GuildId, action, options);
        }
    }
}
