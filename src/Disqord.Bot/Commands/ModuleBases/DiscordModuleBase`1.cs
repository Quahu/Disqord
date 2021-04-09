using System.Collections.Generic;
using System.Threading;
using Disqord.Extensions.Interactivity.Menus;
using Disqord.Extensions.Interactivity.Menus.Paged;
using Disqord.Rest;
using Disqord.Rest.Default;
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

        protected virtual DiscordResponseCommandResult Reply(string content, LocalMentionsBuilder mentions = null)
            => Reply(content, null, mentions);

        protected virtual DiscordResponseCommandResult Reply(LocalEmbedBuilder embed, LocalMentionsBuilder mentions = null)
            => Reply(null, embed, mentions);

        protected virtual DiscordResponseCommandResult Reply(string content, LocalEmbedBuilder embed, LocalMentionsBuilder mentions = null)
            => Response(new LocalMessageBuilder()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions ?? LocalMentionsBuilder.None)
                .WithReply(Context.Message.Id, Context.Message.ChannelId, Context.GuildId, false)
                .Build());

        protected virtual DiscordResponseCommandResult Response(string content, LocalMentionsBuilder mentions = null)
            => Response(content, null, mentions);

        protected virtual DiscordResponseCommandResult Response(LocalEmbedBuilder embed)
            => Response(null, embed, null);

        protected virtual DiscordResponseCommandResult Response(string content, LocalEmbedBuilder embed, LocalMentionsBuilder mentions = null)
            => Response(new LocalMessageBuilder()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions ?? LocalMentionsBuilder.None)
                .Build());

        //protected virtual DiscordCommandResult Response(LocalAttachment attachment)
        //    => Response();

        protected virtual DiscordResponseCommandResult Response(LocalMessage message)
            => new(Context, message);

        protected virtual DiscordReactionCommandResult Reaction(IEmoji emoji)
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
