using System;
using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Bot.Commands.Components;
using Qmmands;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents component command extensions for <see cref="ICommandService"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class ComponentCommandServiceExtensions
{
    /// <summary>
    ///     Enumerates top-level component modules added to this command service.
    /// </summary>
    /// <param name="commandService"> This command service. </param>
    /// <returns>
    ///     An enumerable of the modules.
    /// </returns>
    public static IEnumerable<ComponentModule> EnumerateComponentModules(this ICommandService commandService)
    {
        static IEnumerable<ComponentModule> YieldModules(IEnumerable<IModule> modules)
        {
            foreach (var module in modules)
            {
                yield return (ComponentModule) module;
            }
        }

        foreach (var kvp in commandService.EnumerateModules())
        {
            if (!typeof(ComponentCommandMap).IsAssignableFrom(kvp.Key))
                continue;

            return YieldModules(kvp.Value);
        }

        return Array.Empty<ComponentModule>();
    }
}
