using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class ApplicationCommandPermissionsUpdateHandler : Handler<ApplicationCommandGuildPermissionsJsonModel, ApplicationCommandPermissionsUpdatedEventArgs>
    {
        public override ValueTask<ApplicationCommandPermissionsUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, ApplicationCommandGuildPermissionsJsonModel model)
        {
            var permissions = new TransientApplicationCommandGuildPermissions(Client, model);
            var e = new ApplicationCommandPermissionsUpdatedEventArgs(permissions);
            return new(e);
        }
    }
}
