using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Qmmands;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.Synchronized;

namespace Disqord.Bot.Commands.Application;

public partial class ApplicationCommandMap : ICommandMap
{
    public TopLevelNode GlobalNode { get; }

    public ISynchronizedDictionary<Snowflake, TopLevelNode> GuildNodes { get; }

    public ApplicationCommandMap()
    {
        GlobalNode = new TopLevelNode(this);
        GuildNodes = new SynchronizedDictionary<Snowflake, TopLevelNode>(16);
    }

    public bool CanMap(Type moduleType)
    {
        return typeof(ApplicationModule).IsAssignableFrom(moduleType);
    }

    public ApplicationCommand? FindCommand(IApplicationCommandInteraction interaction)
    {
        var commandGuildId = interaction.CommandGuildId;
        if (interaction is IContextMenuInteraction contextMenuInteraction)
        {
            if (!TryGetTopLevelNode(commandGuildId, out var node))
                return null;

            return node.ContextMenuCommands.GetValueOrDefault(contextMenuInteraction.CommandName);
        }

        if (interaction is ISlashCommandInteraction or IAutoCompleteInteraction)
        {
            var aliases = new FastList<string>(4);
            aliases.Add(interaction.CommandName);

            static void AddAliases(FastList<string> aliases, IReadOnlyDictionary<string, ISlashCommandInteractionOption> options)
            {
                foreach (var (name, option) in options)
                {
                    if (option.Type is SlashCommandOptionType.SubcommandGroup or SlashCommandOptionType.Subcommand)
                    {
                        aliases.Add(name);
                        AddAliases(aliases, option.Options);
                    }

                    break;
                }
            }

            AddAliases(aliases, (interaction as ISlashCommandInteraction)?.Options ?? (interaction as IAutoCompleteInteraction)!.Options);

            if (!TryGetTopLevelNode(commandGuildId, out var node))
                return null;

            return node.FindSlashCommand(aliases);
        }

        return null;
    }

    protected bool TryGetTopLevelNode(Snowflake? commandGuildId, [MaybeNullWhen(false)] out TopLevelNode node)
    {
        if (commandGuildId != null)
        {
            if (!GuildNodes.TryGetValue(commandGuildId.Value, out node))
                return false;
        }
        else
        {
            node = GlobalNode;
        }

        return true;
    }

    protected static bool TryGetGuildIds(IReadOnlyList<ICheck> checks, [MaybeNullWhen(false)] out FastList<Snowflake> guildIds)
    {
        guildIds = null;
        var checkCount = checks.Count;
        for (var i = 0; i < checkCount; i++)
        {
            // TODO: check's group assumption?
            if (checks[i] is RequireGuildAttribute requireGuildAttribute && requireGuildAttribute.Id != null)
                (guildIds ??= new()).Add(requireGuildAttribute.Id.Value);
        }

        return guildIds != null;
    }

    protected TopLevelNode GetOrAddTopLevelNode(Snowflake commandGuildId)
    {
        return GuildNodes.GetOrAdd(commandGuildId, _ => new TopLevelNode(this));
    }

    public void MapModule(IModule module)
    {
        var applicationModule = Guard.IsAssignableToType<ApplicationModule>(module);
        var aliases = new FastList<string>();

        if (!TryGetGuildIds(module.Checks, out var guildIds))
        {
            MapModule(applicationModule, aliases, GlobalNode, null);
        }
        else
        {
            foreach (var guildId in guildIds)
            {
                var node = GetOrAddTopLevelNode(guildId);
                MapModule(applicationModule, aliases, node, guildId);
            }
        }
    }

    protected void MapModule(ApplicationModule module, FastList<string> aliases, TopLevelNode parentNode, Snowflake? parentGuildId)
    {
        var alias = module.Alias;
        if (alias != null)
            aliases.Add(alias);

        foreach (var command in module.Commands)
        {
            aliases.Add(command.Alias);
            if (TryGetGuildIds(command.Checks, out var guildIds))
            {
                foreach (var guildId in guildIds)
                {
                    // TODO: allow switch from multi guild ID parents
                    if (parentGuildId != null && guildId != parentGuildId)
                        throw new ApplicationCommandMappingException($"Commands cannot change the required guild ID of the parent modules ({parentGuildId} -> {guildId}).")
                        {
                            Command = command
                        };

                    var node = parentGuildId == guildId
                        ? parentNode
                        : GetOrAddTopLevelNode(guildId);

                    node.MapCommand(command, aliases);
                }
            }
            else
            {
                parentNode.MapCommand(command, aliases);
            }

            aliases.RemoveAt(aliases.Count - 1);
        }

        foreach (var submodule in module.Submodules)
        {
            if (TryGetGuildIds(submodule.Checks, out var guildIds))
            {
                foreach (var guildId in guildIds)
                {
                    // TODO: allow switch from multi guild ID parents
                    if (parentGuildId != null && guildId != parentGuildId)
                        throw new ApplicationCommandMappingException($"Nested modules cannot change the required guild ID of the parent modules ({parentGuildId} -> {guildId}).")
                        {
                            Module = submodule
                        };

                    var node = parentGuildId == guildId
                        ? parentNode
                        : GetOrAddTopLevelNode(guildId);

                    MapModule(submodule, aliases, node, guildId);
                }
            }
            else
            {
                MapModule(submodule, aliases, parentNode, parentGuildId);
            }
        }

        if (alias != null)
            aliases.RemoveAt(aliases.Count - 1);
    }

    public void UnmapModule(IModule module)
    {
        var applicationModule = Guard.IsAssignableToType<ApplicationModule>(module);
        var aliases = new FastList<string>();

        if (!TryGetGuildIds(module.Checks, out var guildIds))
        {
            UnmapModule(applicationModule, aliases, GlobalNode, null);
        }
        else
        {
            foreach (var guildId in guildIds)
            {
                var node = GetOrAddTopLevelNode(guildId);
                UnmapModule(applicationModule, aliases, node, guildId);
            }
        }
    }

    protected void UnmapModule(ApplicationModule module, FastList<string> aliases, TopLevelNode parentNode, Snowflake? parentGuildId)
    {
        var alias = module.Alias;
        if (alias != null)
            aliases.Add(alias);

        foreach (var command in module.Commands)
        {
            aliases.Add(command.Alias);
            if (TryGetGuildIds(command.Checks, out var guildIds))
            {
                foreach (var guildId in guildIds)
                {
                    if (parentGuildId != null && guildId != parentGuildId)
                        throw new ApplicationCommandMappingException($"Nested commands cannot change the required guild ID of the parent modules ({parentGuildId} -> {guildId}).")
                        {
                            Command = command
                        };

                    var node = parentGuildId == guildId ? parentNode : GetOrAddTopLevelNode(guildId);
                    node.UnmapCommand(command, aliases);
                }
            }
            else
            {
                parentNode.UnmapCommand(command, aliases);
            }

            aliases.RemoveAt(aliases.Count - 1);
        }

        foreach (var submodule in module.Submodules)
        {
            if (TryGetGuildIds(module.Checks, out var guildIds))
            {
                foreach (var guildId in guildIds)
                {
                    if (parentGuildId != null && guildId != parentGuildId)
                        throw new ApplicationCommandMappingException($"Nested modules cannot change the required guild ID of the parent modules ({parentGuildId} -> {guildId}).")
                        {
                            Module = submodule
                        };

                    var node = parentGuildId == guildId ? parentNode : GetOrAddTopLevelNode(guildId);
                    UnmapModule(submodule, aliases, node, guildId);
                }
            }
            else
            {
                UnmapModule(submodule, aliases, parentNode, parentGuildId);
            }
        }

        if (alias != null)
            aliases.RemoveAt(aliases.Count - 1);
    }
}
