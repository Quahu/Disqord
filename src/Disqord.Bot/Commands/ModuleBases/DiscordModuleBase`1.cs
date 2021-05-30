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
        ///     Returns a result that will reply to the context message with the specified content.
        /// </summary>
        /// <param name="content"> The content to reply with. </param>
        /// <param name="mentions"> The mentions to allow in the content. </param>
        /// <returns>
        ///     A <see cref="DiscordResponseCommandResult"/> replying on execution.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(string content, LocalAllowedMentions mentions = null)
            => Reply(content, null, mentions);

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified embed.
        /// </summary>
        /// <param name="embed"> The embed to reply with. </param>
        /// <param name="mentions"> The mentions to allow in the content. </param>
        /// <returns>
        ///     A <see cref="DiscordResponseCommandResult"/> replying on execution.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(LocalEmbed embed, LocalAllowedMentions mentions = null)
            => Reply(null, embed, mentions);

        /// <summary>
        ///     Returns a result that will reply to the context message with the specified content and embed.
        /// </summary>
        /// <param name="content"> The content to reply with. </param>
        /// <param name="embed"> The embed to reply with. </param>
        /// <param name="mentions"> The mentions to allow in the content. </param>
        /// <returns>
        ///     A <see cref="DiscordResponseCommandResult"/> replying on execution.
        /// </returns>
        protected virtual DiscordResponseCommandResult Reply(string content, LocalEmbed embed, LocalAllowedMentions mentions = null)
            => Response(new LocalMessage()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions ?? LocalAllowedMentions.None)
                .WithReply(Context.Message.Id, Context.Message.ChannelId, Context.GuildId, false));

        protected virtual DiscordResponseCommandResult Response(string content, LocalAllowedMentions mentions = null)
            => Response(content, null, mentions);

        protected virtual DiscordResponseCommandResult Response(LocalEmbed embed)
            => Response(null, embed, null);

        protected virtual DiscordResponseCommandResult Response(string content, LocalEmbed embed, LocalAllowedMentions mentions = null)
            => Response(new LocalMessage()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions ?? LocalAllowedMentions.None));

        //protected virtual DiscordCommandResult Response(LocalAttachment attachment)
        //    => Response();

        protected virtual DiscordResponseCommandResult Response(LocalMessage message)
            => new(Context, message);

        protected virtual DiscordReactionCommandResult Reaction(LocalEmoji emoji)
            => new(Context, emoji);

        protected virtual DiscordMenuCommandResult Pages(params Page[] pages)
            => Pages(pages as IEnumerable<Page>);

        protected virtual DiscordMenuCommandResult Pages(IEnumerable<Page> pages)
            => Pages(new DefaultPageProvider(pages));

        protected virtual DiscordMenuCommandResult Pages(IPageProvider pageProvider)
            => Menu(new PagedMenu(Context.Author.Id, pageProvider));

        protected virtual DiscordMenuCommandResult Menu(MenuBase menu)
            => new(Context, menu);

        /// <summary>
        ///     Gets an instance of <see cref="DefaultRestRequestOptions"/> configured with the <see cref="StoppingToken"/>.
        /// </summary>
        protected virtual IRestRequestOptions RequestOptions => new DefaultRestRequestOptions
        {
            CancellationToken = StoppingToken
        };
    }
}
