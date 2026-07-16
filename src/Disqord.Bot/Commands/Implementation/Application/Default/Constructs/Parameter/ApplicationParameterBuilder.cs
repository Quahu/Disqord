using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

public class ApplicationParameterBuilder : IParameterBuilder
{
    public ApplicationCommandBuilder Command { get; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public Type ReflectedType { get; }

    public Optional<object?> DefaultValue { get; set; }

    public Type? CustomTypeParserType { get; set; }

    public IList<IParameterCheck> Checks { get; } = new List<IParameterCheck>();

    public IList<Attribute> CustomAttributes { get; } = new List<Attribute>();

    public ParameterInfo? ParameterInfo { get; }

    ICommandBuilder IParameterBuilder.Command => Command;

    public ApplicationParameterBuilder(ApplicationCommandBuilder command, Type reflectedType)
        : this(command, reflectedType, isRequired: true)
    { }

    public ApplicationParameterBuilder(ApplicationCommandBuilder command, ParameterInfo parameterInfo)
        : this(command, parameterInfo.ParameterType, !parameterInfo.HasDefaultValue)
    {
        ParameterInfo = parameterInfo;

        Name = CommandUtilities.ToKebabCase(parameterInfo.Name);

        if (parameterInfo.HasDefaultValue)
            DefaultValue = parameterInfo.DefaultValue;
    }

    private ApplicationParameterBuilder(ApplicationCommandBuilder command, Type reflectedType, bool isRequired)
    {
        Command = command;
        ReflectedType = reflectedType;

        if (typeof(IMember).IsAssignableFrom(reflectedType))
        {
            if (isRequired)
            {
                var commandChecks = command.Checks;
                var commandCheckCount = commandChecks.Count;
                var needsGuildCheck = true;
                for (var i = 0; i < commandCheckCount; i++)
                {
                    var commandCheck = commandChecks[i];
                    if (commandCheck is RequireGuildAttribute)
                    {
                        needsGuildCheck = false;
                        break;
                    }
                }

                if (needsGuildCheck)
                    Command.Checks.Add(new RequireGuildAttribute());
            }

            Checks.Add(MemberParameterCheck.Instance);
        }
    }

    public ApplicationParameter Build(ApplicationCommand command)
    {
        return new ApplicationParameter(command, this);
    }

    IParameter IParameterBuilder.Build(ICommand command)
        => Build((ApplicationCommand) command);
}
