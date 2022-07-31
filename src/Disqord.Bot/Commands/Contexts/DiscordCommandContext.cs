using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Qommon;
using Qommon.Disposal;

namespace Disqord.Bot.Commands;

internal abstract class DiscordCommandContext<TCommand> : IDiscordCommandContext, IAsyncDisposable
    where TCommand : class, ICommand
{
    /// <inheritdoc/>
    public IServiceProvider Services => _serviceScope.ServiceProvider;

    /// <inheritdoc/>
    public CancellationToken CancellationToken => Bot.StoppingToken;

    /// <summary>
    ///     Gets the locale of the author.
    /// </summary>
    public abstract CultureInfo Locale { get; }

    /// <inheritdoc/>
    ICommandExecutionStep? ICommandContext.ExecutionStep { get; set; }

    /// <inheritdoc cref="ICommandContext.Command"/>
    public TCommand? Command { get; set; }

    /// <inheritdoc/>
    public IDictionary<IParameter, MultiString>? RawArguments { get; set; }

    /// <inheritdoc/>
    public IDictionary<IParameter, object?>? Arguments { get; set; }

    /// <inheritdoc/>
    IModuleBase? ICommandContext.ModuleBase { get; set; }

    /// <inheritdoc/>
    public DiscordBotBase Bot { get; }

    /// <inheritdoc/>
    public abstract CultureInfo? GuildLocale { get; }

    /// <inheritdoc/>
    public Snowflake? GuildId => GetGuildId();

    /// <inheritdoc/>
    public abstract Snowflake ChannelId { get; }

    /// <inheritdoc/>
    public abstract IUser Author { get; }

    ICommand? ICommandContext.Command
    {
        get => Command;
        set => Command = (TCommand?) value;
    }

    private IServiceScope _serviceScope;

    protected DiscordCommandContext(DiscordBotBase bot)
    {
        Bot = bot;

        InitializeScope();
    }

    [MemberNotNull(nameof(_serviceScope))]
    private void InitializeScope()
    {
        _serviceScope = Bot.Services.CreateScope();
        Bot.SetAccessorContext(this);
    }

    protected abstract Snowflake? GetGuildId();

    protected virtual async ValueTask OnReset()
    {
        CommandUtilities.ResetContext(this);

        await RuntimeDisposal.DisposeAsync(_serviceScope).ConfigureAwait(false);

        InitializeScope();
    }

    protected virtual ValueTask OnDispose()
    {
        return RuntimeDisposal.DisposeAsync(_serviceScope);
    }

    ValueTask ICommandContext.ResetAsync()
    {
        return OnReset();
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        return OnDispose();
    }
}
