using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Components;

public class ComponentCommandBuilder : ICommandBuilder
{
    public ComponentModuleBuilder Module { get; }

    public IList<ComponentParameterBuilder> Parameters => _parameters;

    public string? Pattern { get; set; }

    public bool IsRegexPattern { get; set; }

    public ComponentCommandType Type { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public IList<ICheck> Checks { get; } = new List<ICheck>();

    public IList<Attribute> CustomAttributes { get; } = new List<Attribute>();

    public MethodInfo? MethodInfo { get; }

    public ICommandCallback Callback { get; }

    IModuleBuilder ICommandBuilder.Module => Module;

    IList<IParameterBuilder> ICommandBuilder.Parameters => _parameters;

    private readonly UpcastingList<ComponentParameterBuilder, IParameterBuilder> _parameters = new();

    public ComponentCommandBuilder(ComponentModuleBuilder module, ICommandCallback callback)
    {
        Module = module;
        Callback = callback;
    }

    public ComponentCommandBuilder(ComponentModuleBuilder module, MethodInfo methodInfo, ICommandCallback callback)
        : this(module, callback)
    {
        MethodInfo = methodInfo;
    }

    public virtual ComponentCommand Build(ComponentModule module)
    {
        Guard.IsNotNull(Pattern);

        return new ComponentCommand(module, this);
    }

    ICommand ICommandBuilder.Build(IModule module)
        => Build((ComponentModule) module);
}
