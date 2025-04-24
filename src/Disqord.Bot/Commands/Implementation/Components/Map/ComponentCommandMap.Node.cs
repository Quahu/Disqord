using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Qommon;

namespace Disqord.Bot.Commands.Components;

public partial class ComponentCommandMap
{
    protected virtual Node CreateNode()
    {
        return new Node();
    }

    public class Node
    {
        private readonly Subnode _buttonCommandSubnode = new();
        private readonly Subnode _selectionCommandSubnode = new();
        private readonly Subnode _modalCommandSubnode = new();

        public virtual ComponentCommand? FindCommand(ComponentCommandType componentType, string customId, out IEnumerable<MultiString>? rawArguments)
        {
            var subnode = GetSubnode(componentType);
            return subnode.FindCommand(customId, out rawArguments);
        }

        public virtual void AddCommand(ComponentCommand command)
        {
            var subnode = GetSubnode(command.Type);
            subnode.AddCommand(command);
        }

        public virtual void RemoveCommand(ComponentCommand command)
        {
            var subnode = GetSubnode(command.Type);
            subnode.RemoveCommand(command);
        }

        protected virtual Subnode GetSubnode(ComponentCommandType type)
        {
            return type switch
            {
                ComponentCommandType.Button => _buttonCommandSubnode,
                ComponentCommandType.Selection => _selectionCommandSubnode,
                ComponentCommandType.Modal => _modalCommandSubnode,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }

    public class Subnode
    {
        private readonly PrimitivePatternMatcher _primitivePatternMatcher = new();
        private readonly RegexPatternMatcher _regexPatternMatcher = new();

        public virtual ComponentCommand? FindCommand(string customId, out IEnumerable<MultiString>? rawArguments)
        {
            if (_primitivePatternMatcher.TryMatch(customId, out var command, out rawArguments))
                return command;

            if (_regexPatternMatcher.TryMatch(customId, out command, out rawArguments))
                return command;

            rawArguments = null;
            return null;
        }

        protected virtual PatternMatcher GetPatternMatcher(ComponentCommand command)
        {
            if (_primitivePatternMatcher.IsValid(command))
                return _primitivePatternMatcher;

            if (_regexPatternMatcher.IsValid(command))
                return _regexPatternMatcher;

            Throw.ArgumentException($"No pattern matcher found for component command with pattern '{command.Pattern}'.", nameof(command));
            return null!;
        }

        public virtual void AddCommand(ComponentCommand command)
        {
            var patternMatcher = GetPatternMatcher(command);
            patternMatcher.AddCommand(command);
        }

        public virtual void RemoveCommand(ComponentCommand command)
        {
            var patternMatcher = GetPatternMatcher(command);
            patternMatcher.RemoveCommand(command);
        }
    }

    public abstract class PatternMatcher
    {
        public ImmutableArray<ComponentCommand> Commands => _commands;

        private ImmutableArray<ComponentCommand> _commands = ImmutableArray<ComponentCommand>.Empty;

        public abstract bool TryMatch(string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments);

        public abstract bool IsValid(ComponentCommand command);

        public void AddCommand(ComponentCommand command)
        {
            _ = ImmutableInterlocked.Update(ref _commands, static (commands, command) =>
            {
                if (commands.Contains(command))
                {
                    Throw.InvalidOperationException("This component command has already been added.");
                }

                return commands.Add(command);
            }, command);

            OnAddCommand(command);
        }

        protected virtual void OnAddCommand(ComponentCommand command)
        { }

        public void RemoveCommand(ComponentCommand command)
        {
            _ = ImmutableInterlocked.Update(ref _commands, static (commands, command) =>
            {
                var index = commands.IndexOf(command);
                if (index == -1)
                {
                    Throw.InvalidOperationException("This component command has not been added.");
                }

                return commands.RemoveAt(index);
            }, command);

            OnRemoveCommand(command);
        }

        protected virtual void OnRemoveCommand(ComponentCommand command)
        { }
    }

    public abstract class PerCommandPatternMatcher<TPattern> : PatternMatcher
    {
        private ImmutableArray<(ComponentCommand Command, TPattern Pattern)> _patterns = ImmutableArray<(ComponentCommand Command, TPattern Pattern)>.Empty;

        protected override void OnAddCommand(ComponentCommand command)
        {
            _ = ImmutableInterlocked.Update(ref _patterns, static (regexes, state) =>
            {
                var (@this, command) = state;
                foreach (var (regexCommand, regex) in regexes)
                {
                    if (regexCommand == command)
                    {
                        Throw.InvalidOperationException("This component command has already been added.");
                    }

                    // if (regex.ToString() == command.Pattern)
                    // {
                    //     Throw.InvalidOperationException($"A component command with the pattern '{command.Pattern}' has already been added.");
                    // }
                }

                return regexes.Add((command, @this.GetPattern(command)));
            }, (this, command));
        }

        protected override void OnRemoveCommand(ComponentCommand command)
        {
            _ = ImmutableInterlocked.Update(ref _patterns, static (regexes, command) =>
            {
                var index = -1;
                var i = 0;
                foreach (var (regexCommand, _) in regexes)
                {
                    if (regexCommand == command)
                    {
                        index = i;
                        break;
                    }

                    i++;
                }

                if (index == -1)
                {
                    Throw.InvalidOperationException("This component command has not been added.");
                }

                return regexes.RemoveAt(index);
            }, command);
        }

        public override bool TryMatch(string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments)
        {
            return TryMatch(_patterns, customId, out command, out rawArguments);
        }

        protected abstract bool TryMatch(ImmutableArray<(ComponentCommand Command, TPattern Pattern)> commandsAndPatterns,
            string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments);

        protected abstract TPattern GetPattern(ComponentCommand command);
    }

    public class PrimitivePatternMatcher : PerCommandPatternMatcher<PrimitivePatternMatcher.PatternInformation>
    {
        public override bool IsValid(ComponentCommand command)
        {
            return !command.IsRegexPattern;
        }

        protected override bool TryMatch(ImmutableArray<(ComponentCommand Command, PatternInformation Pattern)> commandsAndPatterns,
            string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments)
        {
            var slices = new List<ReadOnlyMemory<char>>(8);
            List<MultiString>? rawArgumentSlices = null;
            var splitter = new PatternSplitter(customId.AsMemory());
            while (splitter.MoveNext())
            {
                slices.Add(splitter.Current);
            }

            var sliceCount = slices.Count;
            if (sliceCount == 0)
            {
                command = null;
                rawArguments = null;
                return false;
            }

            foreach (var (patternCommand, pattern) in commandsAndPatterns)
            {
                command = patternCommand;

                var patternSlices = pattern.Slices;
                if (patternSlices.Length != sliceCount)
                    continue;

                var slicesMatch = true;
                for (var j = 0; j < sliceCount; j++)
                {
                    var slice = slices[j];
                    var patternSlice = patternSlices[j];
                    if (patternSlice.Length == 1 && patternSlice.Span[0] == '*')
                    {
                        (rawArgumentSlices ??= new(8)).Add(slice);
                        continue;
                    }

                    if (!slice.Span.Equals(patternSlice.Span, StringComparison.Ordinal))
                    {
                        slicesMatch = false;
                        break;
                    }
                }

                if (slicesMatch)
                {
                    rawArguments = rawArgumentSlices;
                    return true;
                }
            }

            command = null;
            rawArguments = null;
            return false;
        }

        protected override PatternInformation GetPattern(ComponentCommand command)
        {
            return new PatternInformation(command.Pattern.AsMemory());
        }

        public struct PatternInformation
        {
            public ReadOnlyMemory<char>[] Slices { get; }

            public PatternInformation(ReadOnlyMemory<char> pattern)
            {
                var splitter = new PatternSplitter(pattern);
                var slices = new List<ReadOnlyMemory<char>>(8);
                while (splitter.MoveNext())
                {
                    slices.Add(splitter.Current);
                }

                // TODO: check if there's at least one slice?
                Slices = slices.ToArray();
            }
        }

        protected struct PatternSplitter
        {
            public ReadOnlyMemory<char> Current => _current;

            private ReadOnlyMemory<char> _current;
            private ReadOnlyMemory<char> _text;

            public PatternSplitter(ReadOnlyMemory<char> text)
            {
                _current = default;
                _text = text;
            }

            public bool MoveNext()
            {
                if (_text.IsEmpty)
                    return false;

                do
                {
                    var index = _text.Span.IndexOf(':');
                    if (index == -1)
                    {
                        if (_text.IsEmpty)
                            return false;

                        _current = _text;
                        _text = default;
                        return !_current.IsEmpty;
                    }

                    _current = _text.Slice(0, index);
                    _text = _text.Slice(index + 1);
                }
                while (_current.IsEmpty);

                return true;
            }
        }
    }

    public class RegexPatternMatcher : PerCommandPatternMatcher<Regex>
    {
        public override bool IsValid(ComponentCommand command)
        {
            return command.IsRegexPattern;
        }

        protected override bool TryMatch(ImmutableArray<(ComponentCommand Command, Regex Pattern)> commandsAndPatterns,
            string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments)
        {
            foreach (var (regexCommand, regex) in commandsAndPatterns)
            {
                command = regexCommand;

                // TODO: regex allocations
                var match = regex.Match(customId);
                if (match.Success)
                {
                    var groups = match.Groups;
                    var groupCount = groups.Count - 1;
                    if (groupCount != 0)
                    {
                        var array = new MultiString[groupCount];
                        for (var j = 0; j < groupCount; j++)
                        {
                            var group = groups[j + 1];
                            array[j] = customId.AsMemory(group.Index, group.Length);
                        }

                        rawArguments = array;
                        return true;
                    }
                }
            }

            command = null;
            rawArguments = null;
            return false;
        }

        protected override Regex GetPattern(ComponentCommand command)
        {
            return new Regex(command.Pattern);
        }
    }
}
