using System.Collections.Generic;
using System.Threading;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordModuleBase<T> : ModuleBase<T>
        where T : DiscordCommandContext
    {
        /// <summary>
        ///     Gets the <see cref="DiscordCommandContext.Bot"/>'s stopping token.
        /// </summary>
        protected virtual CancellationToken StoppingToken => Context.Bot.StoppingToken;

        /// <summary>
        ///     Gets a new instance of <see cref="DefaultRestRequestOptions"/> configured with the <see cref="StoppingToken"/>.
        /// </summary>
        protected virtual IRestRequestOptions RequestOptions => new DefaultRestRequestOptions().WithCancellation(StoppingToken);

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
            => Response(message.WithReply(Context.Message.Id, Context.ChannelId, Context.GuildId, false));

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

        //protected virtual DiscordCommandResult Response(LocalAttachment attachment)
        //    => Response();

        protected virtual DiscordReactionCommandResult Reaction(LocalEmoji emoji)
            => new(Context, emoji);

        protected virtual DiscordMenuCommandResult Pages(params Page[] pages)
            => Pages(pages as IEnumerable<Page>);

        protected virtual DiscordMenuCommandResult Pages(IEnumerable<Page> pages)
            => Pages(new ListPageProvider(pages));

        protected virtual DiscordMenuCommandResult Pages(PageProvider pageProvider)
            => View(new PagedView(pageProvider));

        protected virtual DiscordMenuCommandResult View(ViewBase view)
            => new(Context, new InteractiveMenu(Context.Author.Id, view));

        protected virtual DiscordMenuCommandResult Menu(MenuBase menu)
            => new(Context, menu);
    }
}
