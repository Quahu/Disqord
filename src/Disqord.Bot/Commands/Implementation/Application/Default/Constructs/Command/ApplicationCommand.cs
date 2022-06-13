using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon;
using Qommon.Metadata;

namespace Disqord.Bot.Commands.Application;

public class ApplicationCommand : ICommand
{
    public ApplicationModule Module { get; }

    public IReadOnlyList<ApplicationParameter> Parameters { get; }

    public string Alias { get; }

    public ApplicationCommandType Type { get; }

    public string Name { get; }

    public string? Description { get; }

    public IReadOnlyList<ICheck> Checks { get; }

    public IReadOnlyList<Attribute> CustomAttributes { get; }

    public MethodInfo? MethodInfo { get; }

    public ICommandCallback Callback { get; }

    public ApplicationCommand? AutoCompleteCommand { get; }

    IModule ICommand.Module => Module;

    IReadOnlyList<IParameter> ICommand.Parameters => Parameters;

    public ApplicationCommand(ApplicationModule module, ApplicationCommandBuilder builder)
    {
        builder.CopyMetadataTo(this);

        Module = module;

        var parameterBuilders = builder.Parameters;
        var parameterBuilderCount = parameterBuilders.Count;
        var parameters = new ApplicationParameter[parameterBuilderCount];
        for (var i = 0; i < parameterBuilderCount; i++)
        {
            var parameterBuilder = parameterBuilders[i];
            parameters[i] = parameterBuilder.Build(this);
        }

        Parameters = parameters;

        Guard.IsNotNull(builder.Alias);
        Alias = builder.Alias;
        Type = builder.Type;

        Name = builder.Name ?? Alias;
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
        AutoCompleteCommand = builder.AutoCompleteCommand?.Build(Module);
    }
}
