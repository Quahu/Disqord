using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon;
using Qommon.Metadata;

namespace Disqord.Bot.Commands.Components;

public class ComponentCommand : ICommand
{
    public ComponentModule Module { get; }

    public IReadOnlyList<ComponentParameter> Parameters { get; }

    public string Pattern { get; }

    public bool IsRegexPattern { get; }

    public ComponentCommandType Type { get; }

    public string Name { get; }

    public string? Description { get; }

    public IReadOnlyList<ICheck> Checks { get; }

    public IReadOnlyList<Attribute> CustomAttributes { get; }

    public MethodInfo? MethodInfo { get; }

    public ICommandCallback Callback { get; }

    IModule ICommand.Module => Module;

    IReadOnlyList<IParameter> ICommand.Parameters => Parameters;

    public ComponentCommand(ComponentModule module, ComponentCommandBuilder builder)
    {
        builder.CopyMetadataTo(this);

        Module = module;

        var parameterBuilders = builder.Parameters;
        var parameterBuilderCount = parameterBuilders.Count;
        var parameters = new ComponentParameter[parameterBuilderCount];
        for (var i = 0; i < parameterBuilderCount; i++)
        {
            var parameterBuilder = parameterBuilders[i];
            parameters[i] = parameterBuilder.Build(this);
        }

        Parameters = parameters;

        Guard.IsNotNull(builder.Pattern);
        Pattern = builder.Pattern;
        IsRegexPattern = builder.IsRegexPattern;
        Type = builder.Type;

        Name = builder.Name ?? Pattern;
        Description = builder.Description;

        var builderChecks = builder.Checks;
        var builderCheckCount = builderChecks.Count;
        var checks = new ICheck[builderCheckCount];
        for (var i = 0; i < builderCheckCount; i++)
        {
            var check = builderChecks[i];
            checks[i] = check;
        }

        Checks = checks;

        var builderCustomAttributes = builder.CustomAttributes;
        var builderCustomAttributeCount = builderCustomAttributes.Count;
        var customAttributes = new Attribute[builderCustomAttributeCount];
        for (var i = 0; i < builderCustomAttributeCount; i++)
        {
            var customAttribute = builderCustomAttributes[i];
            customAttributes[i] = customAttribute;
        }

        CustomAttributes = customAttributes;

        MethodInfo = builder.MethodInfo;
        Callback = builder.Callback;
    }
}
