using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Qmmands;
using Qommon;
using Qommon.Collections.ThreadSafe;

namespace Disqord.Bot.Commands.Components;

public partial class ComponentCommandMap : ICommandMap
{
    public Node GlobalNode { get; }

    public IThreadSafeDictionary<Snowflake, Node> GuildNodes { get; }

    public ComponentCommandMap()
    {
        GlobalNode = new Node();
        GuildNodes = ThreadSafeDictionary.ConcurrentDictionary.Create<Snowflake, Node>();
    }

    public virtual bool CanMap(Type moduleType)
    {
        return typeof(ComponentModule).IsAssignableFrom(moduleType);
    }

    public virtual ComponentCommand? FindCommand(IUserInteraction interaction, out IEnumerable<MultiString>? rawArguments)
    {
        var commandGuildId = interaction.GuildId;
        var node = commandGuildId == null
            ? GlobalNode
            : GuildNodes.GetValueOrDefault(commandGuildId.Value);

        var (componentType, customId) = interaction switch
        {
            ISelectionComponentInteraction selectionComponentInteraction => (ComponentCommandType.Selection, selectionComponentInteraction.CustomId),
            IComponentInteraction componentInteraction => (componentInteraction.ComponentType switch
            {
                ComponentType.Button => ComponentCommandType.Button,
                _ => throw new ArgumentOutOfRangeException(nameof(interaction), componentInteraction.ComponentType, "Unsupported component type.")
            }, componentInteraction.CustomId),
            IModalSubmitInteraction modalInteraction => (ComponentCommandType.Modal, modalInteraction.CustomId),
            _ => throw new ArgumentOutOfRangeException(nameof(interaction), interaction, "Unsupported component type.")
        };

        if (node != null)
        {
            var command = node.FindCommand(componentType, customId, out rawArguments);
            if (command != null)
                return command;
        }

        return GlobalNode.FindCommand(componentType, customId, out rawArguments);
    }

    protected static bool TryGetGuildIds(ComponentCommand command, [MaybeNullWhen(false)] out List<Snowflake> guildIds)
    {
        guildIds = null;
        foreach (var check in CommandUtilities.EnumerateAllChecks(command))
        {
            // TODO: check's group assumption?
            if (check is RequireGuildAttribute requireGuildAttribute && requireGuildAttribute.Id != null)
                (guildIds ??= new()).Add(requireGuildAttribute.Id.Value);
        }

        return guildIds != null;
    }

    public void MapModule(IModule module)
    {
        foreach (var command in CommandUtilities.EnumerateAllCommands(module))
        {
            var componentCommand = Guard.IsAssignableToType<ComponentCommand>(command);
            if (TryGetGuildIds(componentCommand, out var guildIds))
            {
                foreach (var guildId in guildIds)
                {
                    var node = GuildNodes.GetOrAdd(guildId, _ => CreateNode());
                    node.AddCommand(componentCommand);
                }
            }
            else
            {
                GlobalNode.AddCommand(componentCommand);
            }
        }
    }

    public void UnmapModule(IModule module)
    {
        foreach (var command in CommandUtilities.EnumerateAllCommands(module))
        {
            var componentCommand = Guard.IsAssignableToType<ComponentCommand>(command);
            if (TryGetGuildIds(componentCommand, out var guildIds))
            {
                foreach (var guildId in guildIds)
                {
                    var node = GuildNodes.GetValueOrDefault(guildId);
                    node?.RemoveCommand(componentCommand);
                }
            }
            else
            {
                GlobalNode.RemoveCommand(componentCommand);
            }
        }
    }
}
