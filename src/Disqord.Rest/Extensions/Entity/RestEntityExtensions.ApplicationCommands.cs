using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IApplicationCommand> ModifyAsync(this IApplicationCommand command, Action<ModifyApplicationCommandActionProperties> action, IRestRequestOptions options = null)
        {
            var client = command.GetRestClient();
            return command.GuildId.HasValue
                ? client.ModifyGuildApplicationCommandAsync(command.ApplicationId, command.GuildId.Value, command.Id, action, options)
                : client.ModifyGlobalApplicationCommandAsync(command.ApplicationId, command.Id, action, options);
        }

        public static Task DeleteAsync(this IApplicationCommand command, IRestRequestOptions options = null)
        {
            var client = command.GetRestClient();
            return command.GuildId.HasValue
                ? client.DeleteGuildApplicationCommandAsync(command.ApplicationId, command.GuildId.Value, command.Id, options)
                : client.DeleteGlobalApplicationCommandAsync(command.ApplicationId, command.Id, options);
        }
    }
}
