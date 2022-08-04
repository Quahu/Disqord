using System;
using System.Collections.Generic;
using System.Reflection;
using Qmmands;
using Qommon.Metadata;

namespace Disqord.Bot.Commands.Application;

public class ApplicationModule : IModule
{
    public ApplicationModule? Parent { get; }

    public IReadOnlyList<ApplicationModule> Submodules { get; }

    public IReadOnlyList<ApplicationCommand> Commands { get; }

    public string? Alias { get; }

    public string Name { get; }

    public string? Description { get; }

    public IReadOnlyList<ICheck> Checks { get; }

    public IReadOnlyList<Attribute> CustomAttributes { get; }

    public TypeInfo? TypeInfo { get; }

    IModule? IModule.Parent => Parent;

    IReadOnlyList<IModule> IModule.Submodules => Submodules;

    IReadOnlyList<ICommand> IModule.Commands => Commands;

    public ApplicationModule(ApplicationModule? parent, ApplicationModuleBuilder builder)
    {
        builder.CopyMetadataTo(this);

        Parent = parent;

        var submoduleBuilders = builder.Submodules;
        var submoduleBuilderCount = submoduleBuilders.Count;
        var submodules = new ApplicationModule[submoduleBuilderCount];
        for (var i = 0; i < submoduleBuilderCount; i++)
        {
            var submoduleBuilder = submoduleBuilders[i];
            submodules[i] = submoduleBuilder.Build(this);
        }

        Submodules = submodules;

        var commandBuilders = builder.Commands;
        var commandBuilderCount = commandBuilders.Count;
        var actualCommandBuilderCount = 0;
        for (var i = 0; i < commandBuilderCount; i++)
        {
            var commandBuilder = commandBuilders[i];
            if (commandBuilder.Type == AutoCompleteAttribute.CommandType)
            {
                // Find matching commands for the auto-complete command.
                var commandCustomAttributes = commandBuilder.CustomAttributes;
                var commandCustomAttributeCount = commandCustomAttributes.Count;
                for (var j = 0; j < commandCustomAttributeCount; j++)
                {
                    var commandCustomAttribute = commandCustomAttributes[j];
                    if (commandCustomAttribute is not AutoCompleteAttribute autoCompleteAttribute)
                        continue;

                    ApplicationCommandBuilder? foundCommandBuilder = null;
                    for (var k = 0; k < commandBuilderCount; k++)
                    {
                        if (i == k)
                            continue;

                        var autoCompletedCommandBuilder = commandBuilders[k];
                        if (autoCompletedCommandBuilder.Type != ApplicationCommandType.Slash)
                            continue;

                        if (autoCompletedCommandBuilder.Alias != autoCompleteAttribute.Alias)
                            continue;

                        foundCommandBuilder = autoCompletedCommandBuilder;
                        break;
                    }

                    if (foundCommandBuilder == null)
                        throw new InvalidOperationException($"No matching command '{autoCompleteAttribute.Alias}' found for the auto-complete command in module {builder.TypeInfo?.Name ?? builder.Name ?? GetType().Name}.");

                    foundCommandBuilder.AutoCompleteCommand = commandBuilder;

                    commandCustomAttributes.RemoveAt(j);
                    j--;
                    commandCustomAttributeCount--;
                }
            }
            else
            {
                actualCommandBuilderCount++;
            }
        }

        var commands = new ApplicationCommand[actualCommandBuilderCount];
        var lastIndex = 0;
        for (var i = 0; i < commandBuilderCount; i++)
        {
            var commandBuilder = commandBuilders[i];
            if (commandBuilder.Type == AutoCompleteAttribute.CommandType)
                continue;

            commands[lastIndex++] = commandBuilder.Build(this);
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
