using System.Collections.Generic;
using Qommon;
using Qommon.Collections.Synchronized;

namespace Disqord.Bot.Commands.Application;

public partial class ApplicationCommandMap
{
    public class TopLevelNode : Node
    {
        public ISynchronizedDictionary<string, ApplicationCommand> ContextMenuCommands { get; }

        public TopLevelNode(ApplicationCommandMap map)
            : base(map)
        {
            ContextMenuCommands = new SynchronizedDictionary<string, ApplicationCommand>();
        }

        public ApplicationCommand? FindSlashCommand(IReadOnlyList<string> aliases)
        {
            if (aliases.Count == 0)
                return null;

            return FindSlashCommand(aliases, 0);
        }

        public void MapCommand(ApplicationCommand command, IReadOnlyList<string> aliases)
        {
            Guard.HasSizeGreaterThan(aliases, 0);

            if (command.Type is ApplicationCommandType.User or ApplicationCommandType.Message)
            {
                if (aliases.Count > 1)
                    throw new ApplicationCommandMappingException("Context menu commands cannot be nested.")
                    {
                        Command = command
                    };

                ContextMenuCommands.Add(aliases[0], command);
                return;
            }

            if (aliases.Count > 3)
            {
                throw new ApplicationCommandMappingException("Slash commands cannot be nested more than 3 levels deep.")
                {
                    Command = command
                };
            }

            MapSlashCommand(command, aliases, 0);
        }

        public void UnmapCommand(ApplicationCommand command, IReadOnlyList<string> aliases)
        {
            Guard.HasSizeGreaterThan(aliases, 0);

            if (command.Type is ApplicationCommandType.User or ApplicationCommandType.Message)
            {
                if (aliases.Count > 1)
                    throw new ApplicationCommandMappingException("Context menu commands cannot be nested.")
                    {
                        Command = command
                    };

                ContextMenuCommands.Remove(aliases[0]);
                return;
            }

            UnmapSlashCommand(command, aliases, 0);
        }
    }

    public class Node
    {
        public ApplicationCommandMap Map { get; }

        public ISynchronizedDictionary<string, ApplicationCommand> SlashCommands { get; }

        public ISynchronizedDictionary<string, Node> Nodes { get; }

        public Node(ApplicationCommandMap map)
        {
            Map = map;

            SlashCommands = new SynchronizedDictionary<string, ApplicationCommand>();
            Nodes = new SynchronizedDictionary<string, Node>();
        }

        protected ApplicationCommand? FindSlashCommand(IReadOnlyList<string> aliases, int startIndex)
        {
            var alias = aliases[startIndex];
            if (startIndex == aliases.Count - 1)
                return SlashCommands.GetValueOrDefault(alias);

            if (!Nodes.TryGetValue(alias, out var node))
                return null;

            return node.FindSlashCommand(aliases, startIndex + 1);
        }

        protected void MapSlashCommand(ApplicationCommand command, IReadOnlyList<string> aliases, int startIndex)
        {
            var alias = aliases[startIndex];
            if (startIndex == aliases.Count - 1)
            {
                SlashCommands.Add(alias, command);
                return;
            }

            var node = Nodes.GetOrAdd(alias, _ => new Node(Map));
            node.MapSlashCommand(command, aliases, startIndex + 1);
        }

        protected void UnmapSlashCommand(ApplicationCommand command, IReadOnlyList<string> aliases, int startIndex)
        {
            var alias = aliases[startIndex];
            if (startIndex == aliases.Count - 1)
            {
                SlashCommands.Remove(alias);
                return;
            }

            if (!Nodes.TryGetValue(alias, out var node))
                return;

            node.UnmapSlashCommand(command, aliases, startIndex + 1);
        }
    }
}
