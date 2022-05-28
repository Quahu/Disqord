using System.Reflection;
using Qmmands;
using Qmmands.Default;

namespace Disqord.Bot.Commands.Application;

public class ApplicationCommandReflector : CommandReflectorBase<ApplicationModuleBuilder, ApplicationCommandBuilder, ApplicationParameterBuilder, IDiscordApplicationCommandContext>
{
    protected override ApplicationModuleBuilder CreateModuleBuilder(ApplicationModuleBuilder? parent, TypeInfo typeInfo)
    {
        return new ApplicationModuleBuilder(parent, typeInfo);
    }

    protected override ApplicationCommandBuilder CreateCommandBuilder(ApplicationModuleBuilder module, MethodInfo methodInfo)
    {
        return new ApplicationCommandBuilder(module, methodInfo, ReflectionCommandCallback.Instance);
    }

    protected override ApplicationParameterBuilder CreateParameterBuilder(ApplicationCommandBuilder command, ParameterInfo parameterInfo)
    {
        return new ApplicationParameterBuilder(command, parameterInfo);
    }
}
