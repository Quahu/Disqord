using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Qmmands;

using Module = Qmmands.Module;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase : DiscordClientBase
    {
        protected virtual IEnumerable<Assembly> GetCommandAssemblies()
            => new[] { Assembly.GetEntryAssembly() };

        protected virtual void AddTypeParsers()
        {
            Commands.AddTypeParser(new SnowflakeTypeParser());
            Commands.AddTypeParser(new ColorTypeParser());
        }

        protected virtual void AddModules()
        {
            try
            {
                var modules = new List<Module>();
                foreach (var assembly in GetCommandAssemblies())
                {
                    modules.AddRange(Commands.AddModules(assembly));
                }

                Logger.LogInformation("Added {0} command modules with {1} commands.", modules.Count, modules.SelectMany(x => CommandUtilities.EnumerateAllCommands(x)).Count());
            }
            catch (CommandMappingException ex)
            {
                Logger.LogCritical(ex, "Failed to map command {0} in module {1}:", ex.Command, ex.Command.Module);
                throw;
            }
        }
    }
}