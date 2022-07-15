using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon;
using Qommon.Metadata;

namespace Disqord.Bot.Commands.Application;

public class ApplicationParameter : IParameter
{
    public ApplicationCommand Command { get; }

    public string Name { get; }

    public string? Description { get; }

    public Type ReflectedType { get; }

    public Optional<object?> DefaultValue { get; }

    public Type? CustomTypeParserType { get; }

    public IReadOnlyList<IParameterCheck> Checks { get; }

    public IReadOnlyList<Attribute> CustomAttributes { get; }

    public ParameterInfo? ParameterInfo { get; }

    ICommand IParameter.Command => Command;

    public ApplicationParameter(ApplicationCommand command, ApplicationParameterBuilder builder)
    {
        builder.CopyMetadataTo(this);

        Command = command;

        Name = builder.Name ?? builder.ParameterInfo?.Name ?? "unnamed";
        Description = builder.Description;
        ReflectedType = builder.ReflectedType;
        DefaultValue = builder.DefaultValue;
        CustomTypeParserType = builder.CustomTypeParserType;

        var builderChecks = builder.Checks;
        var builderCheckCount = builderChecks.Count;
        var checks = new IParameterCheck[builderCheckCount];
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

        ParameterInfo = builder.ParameterInfo;
    }
}
