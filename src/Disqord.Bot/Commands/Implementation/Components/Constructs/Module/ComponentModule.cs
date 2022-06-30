using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon.Metadata;

namespace Disqord.Bot.Commands.Components;

public class ComponentModule : IModule
{
    public ComponentModule? Parent { get; }

    public IReadOnlyList<ComponentModule> Submodules { get; }

    public IReadOnlyList<ComponentCommand> Commands { get; }

    public string? Alias { get; }

    public string Name { get; }

    public string? Description { get; }

    public IReadOnlyList<ICheck> Checks { get; }

    public IReadOnlyList<Attribute> CustomAttributes { get; }

    public TypeInfo? TypeInfo { get; }

    IModule? IModule.Parent => Parent;

    IReadOnlyList<IModule> IModule.Submodules => Submodules;

    IReadOnlyList<ICommand> IModule.Commands => Commands;

    public ComponentModule(ComponentModule? parent, ComponentModuleBuilder builder)
    {
        builder.CopyMetadataTo(this);

        Parent = parent;

        var submoduleBuilders = builder.Submodules;
        var submoduleBuilderCount = submoduleBuilders.Count;
        var submodules = new ComponentModule[submoduleBuilderCount];
        for (var i = 0; i < submoduleBuilderCount; i++)
        {
            var submoduleBuilder = submoduleBuilders[i];
            submodules[i] = submoduleBuilder.Build(this);
        }

        Submodules = submodules;

        var commandBuilders = builder.Commands;
        var commandBuilderCount = commandBuilders.Count;
        var commands = new ComponentCommand[commandBuilderCount];
        for (var i = 0; i < commandBuilderCount; i++)
        {
            var commandBuilder = commandBuilders[i];
            commands[i] = commandBuilder.Build(this);
        }

        Commands = commands;

        Alias = builder.Alias;

        Name = builder.Name ?? Alias ?? builder.TypeInfo?.Name ?? "unnamed";
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

        TypeInfo = builder.TypeInfo;
    }
}
