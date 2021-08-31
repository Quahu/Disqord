using System;
using System.Collections.Generic;
using System.Threading;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordModuleBase<T> : ModuleBase<T>
        where T : DiscordCommandContext
    {
        /// <summary>
        ///     Gets a logger for the command currently being executed.
        ///     Returns <see langword="null"/> if accessed before execution.
        /// </summary>
        protected virtual ILogger Logger
        {
            get
            {
                if (_logger != null || Context == null)
                    return _logger;

                return _logger = Context.Services.GetRequiredService<ILoggerFactory>().CreateLogger($"Command '{Context.Command.Name}'");
            }
            set => _logger = value;
        }
        private ILogger _logger;

        /// <inheritdoc cref="DiscordCommandContext.Bot"/>
        protected virtual DiscordBotBase Bot => Context.Bot;

        /// <summary>
        ///     Gets the <see cref="DiscordCommandContext.Bot"/>'s stopping token.
        /// </summary>
        protected virtual CancellationToken StoppingToken => Context.Bot.StoppingToken;

        /// <summary>
        ///     Gets a new instance of <see cref="DefaultRestRequestOptions"/> configured with the <see cref="StoppingToken"/>.
        /// </summary>
        protected virtual IRestRequestOptions RequestOptions => new DefaultRestRequestOptions().WithCancellation(StoppingToken);

        /// <summary>
        ///     Initializes a new <see cref="DiscordModuleBase"/>.
        /// </summary>
        protected DiscordModuleBase()
        { }

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified content.
        /// </summary>
        /// <param name="content"> The content to reply with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(string content)
            => Reply(new LocalMessage().WithContent(content));

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified embeds.
        /// </summary>
        /// <param name="embeds"> The embeds to reply with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(params LocalEmbed[] embeds)
            => Reply(new LocalMessage().WithEmbeds(embeds));

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified content and embeds.
        /// </summary>
        /// <param name="content"> The content to reply with. </param>
        /// <param name="embeds"> The embeds to reply with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(string content, params LocalEmbed[] embeds)
            => Reply(new LocalMessage().WithContent(content).WithEmbeds(embeds));

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified message.
        /// </summary>
        /// <param name="message"> The message to reply with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(LocalMessage message)
            => Response(message.WithReply(Context.Message.Id, Context.ChannelId, Context.GuildId));

        /// <summary>
        ///     Returns a result that will respond in the context channel with the specified content.
        /// </summary>
        /// <param name="content"> The content to respond with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Response(string content)
            => Response(new LocalMessage().WithContent(content));

        /// <summary>
        ///     Returns a result that will respond in the context channel with the specified embeds.
        /// </summary>
        /// <param name="embeds"> The embeds to respond with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Response(params LocalEmbed[] embeds)
            => Response(new LocalMessage().WithEmbeds(embeds));

        /// <summary>
        ///     Returns a result that will respond in the context channel with the specified content and embeds.
        /// </summary>
        /// <param name="content"> The content to respond with. </param>
        /// <param name="embeds"> The embeds to respond with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Response(string content, params LocalEmbed[] embeds)
            => Response(new LocalMessage().WithContent(content).WithEmbeds(embeds));

        /// <summary>
        ///     Returns a result that will respond in the context channel with the specified message.
        /// </summary>
        /// <param name="message"> The message to respond with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordResponseCommandResult Response(LocalMessage message)
        {
            message.AllowedMentions ??= LocalAllowedMentions.None;
            return new(Context, message);
        }

        /// <summary>
        ///     Returns a result that will react to the context message with the specified emoji.
        /// </summary>
        /// <param name="emoji"> The emoji to react with. </param>
        /// <returns>
        ///     The created command result.
        /// </returns>
        protected virtual DiscordReactionCommandResult Reaction(LocalEmoji emoji)
            => new(Context, emoji);

        /// <inheritdoc cref="Pages(IEnumerable{Page},TimeSpan)"/>
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
        protected virtual DiscordMenuCommandResult View(ViewBase view, TimeSpan timeout = default)
            => Menu(new DefaultMenu(view)
            {
                AuthorId = Context.Author.Id
            }, timeout);

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
}
