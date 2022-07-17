using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IApplicationCommand> ModifyAsync(this IApplicationCommand command,
        Action<ModifyApplicationCommandActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = command.GetRestClient();
        return command.GuildId != null
            ? client.ModifyGuildApplicationCommandAsync(command.ApplicationId, command.GuildId.Value, command.Id, action, options, cancellationToken)
            : client.ModifyGlobalApplicationCommandAsync(command.ApplicationId, command.Id, action, options, cancellationToken);
    }

    public static Task DeleteAsync(this IApplicationCommand command,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = command.GetRestClient();
        return command.GuildId != null
            ? client.DeleteGuildApplicationCommandAsync(command.ApplicationId, command.GuildId.Value, command.Id, options, cancellationToken)
            : client.DeleteGlobalApplicationCommandAsync(command.ApplicationId, command.Id, options, cancellationToken);
    }
}