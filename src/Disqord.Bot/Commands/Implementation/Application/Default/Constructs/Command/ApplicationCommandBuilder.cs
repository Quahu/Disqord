using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

public class ApplicationCommandBuilder : ICommandBuilder
{
    public ApplicationModuleBuilder Module { get; }

    public IList<ApplicationParameterBuilder> Parameters => _parameters;

    public string? Alias { get; set; }

    public ApplicationCommandType Type { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public IList<ICheck> Checks { get; } = new List<ICheck>();

    public IList<Attribute> CustomAttributes { get; } = new List<Attribute>();

    public MethodInfo? MethodInfo { get; }

    public ICommandCallback Callback { get; }

    public ApplicationCommandBuilder? AutoCompleteCommand { get; set; }

    IModuleBuilder ICommandBuilder.Module => Module;

    IList<IParameterBuilder> ICommandBuilder.Parameters => _parameters;

    private readonly UpcastingList<ApplicationParameterBuilder, IParameterBuilder> _parameters = new();

    public ApplicationCommandBuilder(ApplicationModuleBuilder module, ICommandCallback callback)
    {
        Module = module;
        Callback = callback;
    }

    public ApplicationCommandBuilder(ApplicationModuleBuilder module, MethodInfo methodInfo, ICommandCallback callback)
        : this(module, callback)
    {
        MethodInfo = methodInfo;
    }

    public virtual ApplicationCommand Build(ApplicationModule module)
    {
        Guard.IsNotNull(Alias);

        if (Type == ApplicationCommandType.User)
        {
            if (_parameters.Count != 1 && !typeof(IUser).IsAssignableFrom(_parameters[0].ReflectedType))
                Throw.InvalidOperationException("User commands must take a single user or member parameter.");
        }
        else if (Type == ApplicationCommandType.Message)
        {
            if (_parameters.Count != 1 && !typeof(IMessage).IsAssignableFrom(_parameters[0].ReflectedType))
                Throw.InvalidOperationException("User commands must take a single message parameter.");
        }

        return new ApplicationCommand(module, this);
    }

    ICommand ICommandBuilder.Build(IModule module)
        => Build((ApplicationModule) module);
}
