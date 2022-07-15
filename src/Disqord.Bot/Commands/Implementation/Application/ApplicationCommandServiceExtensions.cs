using System;
using System.Collections.Generic;
using System.ComponentModel;
using Qmmands;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents application command extensions for <see cref="ICommandService"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ApplicationCommandServiceExtensions
{
    /// <summary>
    ///     Enumerates top-level application modules added to this command service.
    /// </summary>
    /// <param name="commandService"> This command service. </param>
    /// <returns>
    ///     An enumerable of the modules.
    /// </returns>
    public static IEnumerable<ApplicationModule> EnumerateApplicationModules(this ICommandService commandService)
    {
        static IEnumerable<ApplicationModule> YieldModules(IEnumerable<IModule> modules)
        {
            foreach (var module in modules)
            {
                yield return (ApplicationModule) module;
            }
        }

        foreach (var kvp in commandService.EnumerateModules())
        {
            if (!typeof(ApplicationCommandMap).IsAssignableFrom(kvp.Key))
                continue;

            return YieldModules(kvp.Value);
        }

        return Array.Empty<ApplicationModule>();
    }
}
