using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Components;

public class ComponentParameterBuilder : IParameterBuilder
{
    public ComponentCommandBuilder Command { get; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public Type ReflectedType { get; }

    public Optional<object?> DefaultValue { get; set; }

    public Type? CustomTypeParserType { get; set; }

    public IList<IParameterCheck> Checks { get; } = new List<IParameterCheck>();

    public IList<Attribute> CustomAttributes { get; } = new List<Attribute>();

    public ParameterInfo? ParameterInfo { get; }

    ICommandBuilder IParameterBuilder.Command => Command;

    public ComponentParameterBuilder(ComponentCommandBuilder command, Type reflectedType)
    {
        Command = command;
        ReflectedType = reflectedType;
    }

    public ComponentParameterBuilder(ComponentCommandBuilder command, ParameterInfo parameterInfo)
        : this(command, parameterInfo.ParameterType)
    {
        ParameterInfo = parameterInfo;

        Name = parameterInfo.Name;
        if (parameterInfo.HasDefaultValue)
            DefaultValue = parameterInfo.DefaultValue;
    }

    public ComponentParameter Build(ComponentCommand command)
    {
        return new ComponentParameter(command, this);
    }

    IParameter IParameterBuilder.Build(ICommand command)
        => Build((ComponentCommand) command);
}
