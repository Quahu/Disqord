using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Qmmands;
using Qmmands.Delegates;
using Qommon.Collections;
using Qommon.Events;
using Module = Qmmands.Module;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase : DiscordClientBase, ICommandService
    {
        public StringComparison StringComparison => _commandService.StringComparison;

        public RunMode DefaultRunMode => _commandService.DefaultRunMode;

        public bool IgnoresExtraArguments => _commandService.IgnoresExtraArguments;

        public string Separator => _commandService.Separator;

        public SeparatorRequirement SeparatorRequirement => _commandService.SeparatorRequirement;

        public IArgumentParser DefaultArgumentParser => _commandService.DefaultArgumentParser;

        public CooldownBucketKeyGeneratorDelegate CooldownBucketKeyGenerator => _commandService.CooldownBucketKeyGenerator;

        public IReadOnlyDictionary<char, char> QuotationMarkMap => _commandService.QuotationMarkMap;

        public IReadOnlyList<string> NullableNouns => _commandService.NullableNouns;

        public ReadOnlySet<Module> TopLevelModules => _commandService.TopLevelModules;

        public event AsynchronousEventHandler<CommandExecutedEventArgs> CommandExecuted
        {
            add => _commandService.CommandExecuted += value;
            remove => _commandService.CommandExecuted -= value;
        }

        public event AsynchronousEventHandler<CommandExecutionFailedEventArgs> CommandExecutionFailed
        {
            add => _commandService.CommandExecutionFailed += value;
            remove => _commandService.CommandExecutionFailed -= value;
        }

        public Module AddModule(Action<ModuleBuilder> action)
            => _commandService.AddModule(action);

        public void AddModule(Module module)
            => _commandService.AddModule(module);

        public Module AddModule<TModule>(Action<ModuleBuilder> action = null)
            => _commandService.AddModule<TModule>(action);

        public Module AddModule(Type type, Action<ModuleBuilder> action = null)
            => _commandService.AddModule(type, action);

        public IReadOnlyList<Module> AddModules(Assembly assembly, Predicate<TypeInfo> predicate = null, Action<ModuleBuilder> action = null)
            => _commandService.AddModules(assembly, predicate, action);

        public void SetDefaultArgumentParser<T>() where T : IArgumentParser
            => _commandService.SetDefaultArgumentParser<T>();

        public void SetDefaultArgumentParser(Type type)
            => _commandService.SetDefaultArgumentParser(type);

        public void SetDefaultArgumentParser(IArgumentParser parser)
            => _commandService.SetDefaultArgumentParser(parser);

        public void AddArgumentParser(IArgumentParser parser)
            => _commandService.AddArgumentParser(parser);

        public void RemoveArgumentParser<T>() where T : IArgumentParser
            => _commandService.RemoveArgumentParser<T>();

        public void RemoveArgumentParser(Type type)
            => _commandService.RemoveArgumentParser(type);

        public IArgumentParser GetArgumentParser<T>() where T : IArgumentParser
            => _commandService.GetArgumentParser<T>();

        public IArgumentParser GetArgumentParser(Type type)
            => _commandService.GetArgumentParser(type);

        public void AddTypeParser<T>(TypeParser<T> parser, bool replacePrimitive = false)
            => _commandService.AddTypeParser(parser, replacePrimitive);

        public Task<IResult> ExecuteAsync(string input, CommandContext context)
            => _commandService.ExecuteAsync(input, context);

        public Task<IResult> ExecuteAsync(Command command, string rawArguments, CommandContext context)
            => _commandService.ExecuteAsync(command, rawArguments, context);

        public IReadOnlyList<CommandMatch> FindCommands(string path)
            => _commandService.FindCommands(path);

        public IReadOnlyList<Command> GetAllCommands()
            => _commandService.GetAllCommands();

        public IReadOnlyList<Module> GetAllModules()
            => _commandService.GetAllModules();

        public TParser GetSpecificTypeParser<T, TParser>() where TParser : TypeParser<T>
            => _commandService.GetSpecificTypeParser<T, TParser>();

        public TypeParser<T> GetTypeParser<T>(bool replacingPrimitive = false)
            => _commandService.GetTypeParser<T>(replacingPrimitive);

        public void RemoveAllTypeParsers()
            => _commandService.RemoveAllTypeParsers();

        public void RemoveAllModules()
            => _commandService.RemoveAllModules();

        public void RemoveModule(Module module)
            => _commandService.RemoveModule(module);

        public void RemoveTypeParser<T>(TypeParser<T> parser)
            => _commandService.RemoveTypeParser(parser);
    }
}
