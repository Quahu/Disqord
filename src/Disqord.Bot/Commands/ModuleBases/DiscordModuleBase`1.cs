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
        ///     A <see cref="DiscordResponseCommandResult"/> replying on execution.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(string content)
            => Reply(new LocalMessage().WithContent(content));

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified embed.
        /// </summary>
        /// <param name="embeds"> The embeds to reply with. </param>
        /// <returns>
        ///     A <see cref="DiscordResponseCommandResult"/> replying on execution.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(params LocalEmbed[] embeds)
            => Reply(new LocalMessage().WithEmbeds(embeds));

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified content and embed.
        /// </summary>
        /// <param name="content"> The content to reply with. </param>
        /// <param name="embeds"> The embeds to reply with. </param>
        /// <returns>
        ///     A <see cref="DiscordResponseCommandResult"/> replying on execution.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(string content, params LocalEmbed[] embeds)
            => Reply(new LocalMessage().WithContent(content).WithEmbeds(embeds));

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified content and embed.
        /// </summary>
        /// <param name="message"> The message to reply with. </param>
        /// <returns>
        ///     A <see cref="DiscordResponseCommandResult"/> replying on execution.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(LocalMessage message)
            => Response(message.WithReply(Context.Message.Id, Context.ChannelId, Context.GuildId));

        protected virtual DiscordResponseCommandResult Response(string content)
            => Response(new LocalMessage().WithContent(content));

        protected virtual DiscordResponseCommandResult Response(params LocalEmbed[] embeds)
            => Response(new LocalMessage().WithEmbeds(embeds));

        protected virtual DiscordResponseCommandResult Response(string content, params LocalEmbed[] embeds)
            => Response(new LocalMessage().WithContent(content).WithEmbeds(embeds));

        protected virtual DiscordResponseCommandResult Response(LocalMessage message)
        {
            message.AllowedMentions ??= LocalAllowedMentions.None;
            return new(Context, message);
        }

        protected virtual DiscordReactionCommandResult Reaction(LocalEmoji emoji)
            => new(Context, emoji);

        protected virtual DiscordMenuCommandResult Pages(params Page[] pages)
            => Pages(pages as IEnumerable<Page>);

        protected virtual DiscordMenuCommandResult Pages(IEnumerable<Page> pages, TimeSpan timeout = default)
            => Pages(new ListPageProvider(pages), timeout);

        protected virtual DiscordMenuCommandResult Pages(PageProvider pageProvider, TimeSpan timeout = default)
            => View(new PagedView(pageProvider), timeout);

        protected virtual DiscordMenuCommandResult View(ViewBase view, TimeSpan timeout = default)
            => Menu(new InteractiveMenu(Context.Author.Id, view), timeout);

        protected virtual DiscordMenuCommandResult Menu(MenuBase menu, TimeSpan timeout = default)
            => new(Context, menu, timeout);
    }
}
