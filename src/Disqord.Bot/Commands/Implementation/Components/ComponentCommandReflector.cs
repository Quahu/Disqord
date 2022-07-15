using System.Reflection;
using Qmmands;
using Qmmands.Default;

namespace Disqord.Bot.Commands.Components;

public class ComponentCommandReflector : CommandReflectorBase<ComponentModuleBuilder, ComponentCommandBuilder, ComponentParameterBuilder, IDiscordComponentCommandContext>
{
    /// <inheritdoc />
    public ComponentCommandReflector(ICommandReflectorCallbackProvider callbackProvider)
        : base(callbackProvider)
    { }

    /// <inheritdoc/>
    protected override ComponentModuleBuilder CreateModuleBuilder(ComponentModuleBuilder? parent, TypeInfo typeInfo)
    {
        return new ComponentModuleBuilder(parent, typeInfo);
    }

    /// <inheritdoc/>
    protected override ComponentCommandBuilder CreateCommandBuilder(ComponentModuleBuilder module, MethodInfo methodInfo)
    {
        return new ComponentCommandBuilder(module, methodInfo, GetCallback(methodInfo));
    }

    /// <inheritdoc/>
    protected override ComponentParameterBuilder CreateParameterBuilder(ComponentCommandBuilder command, ParameterInfo parameterInfo)
    {
        return new ComponentParameterBuilder(command, parameterInfo);
    }
}
