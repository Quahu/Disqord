using System.Net;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<RestInvite> GetInviteAsync(string code, bool withCounts = true, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetInviteAsync(code, withCounts, options).ConfigureAwait(false);
                return new RestInvite(this, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound || ex.JsonErrorCode == JsonErrorCode.InviteCodeIsEitherInvalidOrTaken)
            {
                return null;
            }
        }

        public async Task<RestInvite> DeleteInviteAsync(string code, RestRequestOptions options = null)
        {
            var model = await ApiClient.DeleteInviteAsync(code, options).ConfigureAwait(false);
            return new RestInvite(this, model);
        }
    }
}
