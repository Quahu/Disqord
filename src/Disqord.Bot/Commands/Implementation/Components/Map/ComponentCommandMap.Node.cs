using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.Synchronized;

namespace Disqord.Bot.Commands.Components;

public partial class ComponentCommandMap
{
    protected virtual Node CreateNode()
        => new();

    public class Node
    {
        public ISynchronizedDictionary<ComponentCommandType, Subnode> Subnodes { get; }

        public Node()
        {
            Subnodes = new SynchronizedDictionary<ComponentCommandType, Subnode>();
        }

        protected virtual Subnode CreateSubnode()
            => new();

        public virtual ComponentCommand? FindCommand(ComponentCommandType componentType, string customId, out IEnumerable<MultiString>? rawArguments)
        {
            var subnode = Subnodes.GetValueOrDefault(componentType);
            if (subnode == null)
            {
                rawArguments = null;
                return null;
            }

            return subnode.FindCommand(customId, out rawArguments);
        }

        public virtual void AddCommand(ComponentCommand command)
        {
            var subnode = Subnodes.GetOrAdd(command.Type, _ => CreateSubnode());
            subnode.AddCommand(command);
        }

        public virtual void RemoveCommand(ComponentCommand command)
        {
            var subnode = Subnodes.GetValueOrDefault(command.Type);
            subnode?.RemoveCommand(command);
        }
    }

    public class Subnode
    {
        public FastList<ComponentCommand> Commands { get; }

        protected FastList<PatternMatcher> PatternMatchers;

        public Subnode()
        {
            Commands = new FastList<ComponentCommand>();

            PatternMatchers = new(2)
            {
                new PrimitivePatternMatcher(),
                new RegexPatternMatcher()
            };
        }

        public virtual ComponentCommand? FindCommand(string customId, out IEnumerable<MultiString>? rawArguments)
        {
            lock (PatternMatchers)
            {
                var patternMatchers = PatternMatchers;
                var patternMatcherCount = patternMatchers.Count;
                for (var i = 0; i < patternMatcherCount; i++)
                {
                    var patternMatcher = patternMatchers[i];
                    if (patternMatcher.TryMatch(customId, out var command, out rawArguments))
                        return command;
                }
            }

            rawArguments = null;
            return null;
        }

        private PatternMatcher GetPatternMatcher(ComponentCommand command)
        {
            var patternMatchers = PatternMatchers;
            var patternMatcherCount = patternMatchers.Count;
            for (var i = 0; i < patternMatcherCount; i++)
            {
                var patternMatcher = patternMatchers[i];
                if (patternMatcher.IsValid(command))
                    return patternMatcher;
            }

            Throw.ArgumentException($"No pattern matcher found for component command with pattern '{command.Pattern}'.", nameof(command));
            return null!;
        }

        public virtual void AddCommand(ComponentCommand command)
        {
            lock (PatternMatchers)
            {
                var patternMatcher = GetPatternMatcher(command);
                patternMatcher.AddCommand(command);
            }
        }

        public virtual void RemoveCommand(ComponentCommand command)
        {
            lock (PatternMatchers)
            {
                var patternMatcher = GetPatternMatcher(command);
                patternMatcher.RemoveCommand(command);
            }
        }
    }

    public abstract class PatternMatcher
    {
        protected FastList<ComponentCommand> Commands;

        protected PatternMatcher()
        {
            Commands = new();
        }

        public abstract bool TryMatch(string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments);

        public abstract bool IsValid(ComponentCommand command);

        protected virtual void OnAddCommand(ComponentCommand command)
        { }

        public void AddCommand(ComponentCommand command)
        {
            if (Commands.Contains(command))
                throw new InvalidOperationException("This component command has already been added.");

            Commands.Add(command);

            OnAddCommand(command);
        }

        protected virtual void OnRemoveCommand(ComponentCommand command, int index)
        { }

        public void RemoveCommand(ComponentCommand command)
        {
            var index = Commands.IndexOf(command);
            if (index == -1)
                throw new InvalidOperationException("This component command has not been added.");

            Commands.RemoveAt(index);

            OnRemoveCommand(command, index);
        }
    }

    public class PrimitivePatternMatcher : PatternMatcher
    {
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

        protected struct PatternInformation
        {
            public ReadOnlyMemory<char>[] Slices { get; }

            public PatternInformation(ReadOnlyMemory<char> pattern)
            {
                var splitter = new PatternSplitter(pattern);
                var slices = new FastList<ReadOnlyMemory<char>>(8);
                while (splitter.MoveNext())
                {
                    slices.Add(splitter.Current);
                }

                // TODO: check if there's at least one slice?
                Slices = slices.ToArray();
            }
        }

        protected FastList<PatternInformation> Patterns { get; }

        public PrimitivePatternMatcher()
        {
            Patterns = new();
        }

        /// <inheritdoc />
        public override bool TryMatch(string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments)
        {
            var slices = new FastList<ReadOnlyMemory<char>>(8);
            FastList<MultiString>? rawArgumentSlices = null;
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

            var commands = Commands;
            var commandCount = commands.Count;
            var patterns = Patterns;
            for (var i = 0; i < commandCount; i++)
            {
                command = commands[i];
                var pattern = patterns[i];
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

        /// <inheritdoc />
        public override bool IsValid(ComponentCommand command)
        {
            return !command.IsRegexPattern;
        }

        /// <inheritdoc />
        protected override void OnAddCommand(ComponentCommand command)
        {
            Patterns.Add(new PatternInformation(command.Pattern.AsMemory()));
        }

        /// <inheritdoc />
        protected override void OnRemoveCommand(ComponentCommand command, int index)
        {
            Patterns.RemoveAt(index);
        }
    }

    public class RegexPatternMatcher : PatternMatcher
    {
        protected FastList<Regex> Regexes { get; }

        public RegexPatternMatcher()
        {
            Regexes = new();
        }

        /// <inheritdoc />
        public override bool TryMatch(string customId, [MaybeNullWhen(false)] out ComponentCommand command, out IEnumerable<MultiString>? rawArguments)
        {
            var commands = Commands;
            var commandCount = commands.Count;
            var regexes = Regexes;
            for (var i = 0; i < commandCount; i++)
            {
                command = commands[i];
                var regex = regexes[i];

                // TODO: regex allocations
                var match = regex.Match(customId);
                if (match.Success)
                {
                    var groups = match.Groups;
                    var groupCount = groups.Count;
                    var array = new MultiString[groupCount];
                    for (var j = 0; j < groupCount; j++)
                    {
                        var group = groups[j];
                        array[j] = customId.AsMemory(group.Index, group.Length);
                    }

                    rawArguments = array;
                    return true;
                }
            }

            command = null;
            rawArguments = null;
            return false;
        }

        /// <inheritdoc />
        public override bool IsValid(ComponentCommand command)
        {
            return command.IsRegexPattern;
        }

        /// <inheritdoc />
        protected override void OnAddCommand(ComponentCommand command)
        {
            Regexes.Add(new Regex(command.Pattern));
        }

        /// <inheritdoc />
        protected override void OnRemoveCommand(ComponentCommand command, int index)
        {
            Regexes.RemoveAt(index);
        }
    }
}
