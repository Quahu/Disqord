using System;
using System.Collections.Generic;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot.Commands;

public abstract class DiscordModuleBase<TContext> : ModuleBase<TContext>
    where TContext : IDiscordCommandContext
{
    /// <summary>
    ///     Gets a logger for the currently executed command.
    /// </summary>
    /// <remarks>
    ///     Can return <see langword="null"/> if accessed inside the constructor.
    /// </remarks>
    protected virtual ILogger Logger
    {
        get
        {
            var logger = _logger;
            if (logger != null || Context == null)
                return logger!;

            var loggerFactory = Context.Services.GetRequiredService<ILoggerFactory>();
            return _logger = loggerFactory.CreateLogger($"Command '{Context.Command!.Name}'");
        }
        set => _logger = value;
    }
    private ILogger? _logger;

    /// <inheritdoc cref="IDiscordCommandContext.Bot"/>
    protected virtual DiscordBotBase Bot => Context.Bot;

    /// <summary>
    ///     Initializes a new <see cref="DiscordModuleBase{TCommandContext}"/>.
    /// </summary>
    private protected DiscordModuleBase()
    { }

    /// <inheritdoc cref="Pages(System.Collections.Generic.IEnumerable{Disqord.Extensions.Interactivity.Menus.Paged.Page},TimeSpan)"/>
    protected virtual DiscordMenuCommandResult Pages(params Page[] pages)
        => Pages(pages as IEnumerable<Page>);

    /// <summary>
    ///     Returns a result that will start a <see cref="PagedView"/> in the context channel with the specified pages.
    /// </summary>
    /// <param name="pages"> The pages for the paged view. </param>
    /// <param name="timeout"> The timeout for the menu. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordMenuCommandResult Pages(IEnumerable<Page> pages, TimeSpan timeout = default)
        => Pages(new ListPageProvider(pages), timeout);

    /// <summary>
    ///     Returns a result that will start a <see cref="PagedView"/> in the context channel with the specified page provider.
    /// </summary>
    /// <param name="pageProvider"> The page provider for the paged view. </param>
    /// <param name="timeout"> The timeout for the menu. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordMenuCommandResult Pages(PageProvider pageProvider, TimeSpan timeout = default)
        => View(new PagedView(pageProvider), timeout);

    /// <summary>
    ///     Returns a result that will start the specified view in the context channel.
    /// </summary>
    /// <param name="view"> The view to start. </param>
    /// <param name="timeout"> The timeout for the menu. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected abstract DiscordMenuCommandResult View(ViewBase view, TimeSpan timeout = default);

    /// <summary>
    ///     Returns a result that will start the specified menu in the context channel.
    /// </summary>
    /// <param name="menu"> The menu to start. </param>
    /// <param name="timeout"> The timeout for the menu. </param>
    /// <returns>
    ///     The created command result.
    /// </returns>
    protected virtual DiscordMenuCommandResult Menu(MenuBase menu, TimeSpan timeout = default)
        => new(Context, menu, timeout);
}
