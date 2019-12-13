using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Bot.Parsers;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Bot
{
    public partial class DiscordBot : DiscordClient, IServiceProvider
    {
        public IReadOnlyList<string> Prefixes { get; private set; }

        public bool HasMentionPrefix { get; private set; }

        private CommandService _commandService;
        private IServiceProvider _provider;

        public DiscordBot(RestDiscordClient restClient, DiscordBotConfiguration configuration = null) : base(restClient, configuration)
            => Setup(configuration);

        public DiscordBot(TokenType tokenType, string token, DiscordBotConfiguration configuration = null) : base(tokenType, token, configuration)
            => Setup(configuration);

        private void Setup(DiscordBotConfiguration configuration)
        {
            configuration = configuration ?? DiscordBotConfiguration.Default;
            _commandService = configuration.CommandService ?? new CommandService();
            _provider = configuration.ProviderFactory?.Invoke(this);
            Prefixes = configuration.Prefixes?.ToImmutableArray() ?? ImmutableArray<string>.Empty;
            HasMentionPrefix = configuration.HasMentionPrefix;
            AddTypeParser(CachedRoleParser.Instance);
            AddTypeParser(CachedMemberParser.Instance);
            AddTypeParser(CachedUserParser.Instance);
            AddTypeParser(CachedGuildChannelParser<CachedGuildChannel>.Instance);
            AddTypeParser(CachedGuildChannelParser<CachedTextChannel>.Instance);
            AddTypeParser(CachedGuildChannelParser<CachedVoiceChannel>.Instance);
            AddTypeParser(CachedGuildChannelParser<CachedCategoryChannel>.Instance);
            AddTypeParser(LocalCustomEmojiParser.Instance);
            AddTypeParser(SnowflakeParser.Instance);
            AddTypeParser(ColorParser.Instance);
            //AddTypeParser(SanitaryContentParser.Instance);

            MessageReceived += MessageReceivedAsync;
        }

        protected virtual ValueTask<bool> CheckMessageAsync(CachedUserMessage message)
            => new ValueTask<bool>(IsBot
                ? !message.Author.IsBot
                : message.Author.Id == CurrentUser.Id);

        protected virtual ValueTask<bool> BeforeExecutedAsync(DiscordCommandContext context)
            => new ValueTask<bool>(true);

        protected virtual ValueTask AfterExecutedAsync(IResult result, DiscordCommandContext context)
            => default;

        protected virtual ValueTask<(string Prefix, string Output)> FindPrefixAsync(CachedUserMessage message)
        {
            if (CommandUtilities.HasAnyPrefix(message.Content, Prefixes, out var prefix, out var output))
                return new ValueTask<(string, string)>((prefix, output));

            return default;
        }

        protected virtual ValueTask<DiscordCommandContext> GetCommandContextAsync(CachedUserMessage message, string prefix)
            => new ValueTask<DiscordCommandContext>(new DiscordCommandContext(this, prefix, message));

        private async Task MessageReceivedAsync(MessageReceivedEventArgs args)
        {
            if (!(args.Message is CachedUserMessage message))
                return;

            try
            {
                if (!await CheckMessageAsync(message).ConfigureAwait(false))
                    return;
            }
            catch (Exception ex)
            {
                Log(LogMessageSeverity.Error, "An exception occurred while running the check message callback.", ex);
                return;
            }

            (string Prefix, string Output) prefixResult;
            try
            {
                prefixResult = await FindPrefixAsync(message).ConfigureAwait(false);
                if (prefixResult == default)
                    return;
            }
            catch (Exception ex)
            {
                Log(LogMessageSeverity.Error, "An exception occurred while finding the prefix.", ex);
                return;
            }

            DiscordCommandContext context;
            try
            {
                context = await GetCommandContextAsync(message, prefixResult.Prefix).ConfigureAwait(false);
                if (context == null)
                    return;
            }
            catch (Exception ex)
            {
                Log(LogMessageSeverity.Error, "An exception occurred while getting the context.", ex);
                return;
            }

            try
            {
                if (!await BeforeExecutedAsync(context).ConfigureAwait(false))
                    return;
            }
            catch (Exception ex)
            {
                Log(LogMessageSeverity.Error, "An exception occurred while running the before executed callback.", ex);
                return;
            }

            var result = await _commandService.ExecuteAsync(prefixResult.Output, context).ConfigureAwait(false);
            try
            {
                await AfterExecutedAsync(result, context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log(LogMessageSeverity.Error, "An exception occurred while running the after executed callback.", ex);
            }
        }

        public virtual object GetService(Type serviceType)
        {
            if (serviceType == typeof(DiscordBot) || serviceType == GetType())
                return this;

            return _provider?.GetService(serviceType);
        }


        /// <exception cref="TaskCanceledException"></exception>
        public void Run(CancellationToken cancellationToken = default)
            => RunAsync(cancellationToken).GetAwaiter().GetResult();

        internal new void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Bot", severity, message, exception));

        public override ValueTask DisposeAsync()
        {
            MessageReceived -= MessageReceivedAsync;
            (_provider as IDisposable)?.Dispose();
            return base.DisposeAsync();
        }
    }
}