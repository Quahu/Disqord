﻿using System.Threading;
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

        protected virtual DiscordCommandResult Reply(string content, LocalMentionsBuilder mentions = null)
            => Reply(content, null, mentions);

        protected virtual DiscordCommandResult Reply(LocalEmbedBuilder embed)
            => Reply(null, embed, null);

        protected virtual DiscordCommandResult Reply(string content, LocalEmbedBuilder embed, LocalMentionsBuilder mentions = null)
            => Response(new LocalMessageBuilder()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions)
                .WithReply(Context.Message.Id, Context.Message.ChannelId)
                .Build());

        protected virtual DiscordCommandResult Response(string content, LocalMentionsBuilder mentions = null)
            => Response(content, null, mentions);

        protected virtual DiscordCommandResult Response(LocalEmbedBuilder embed)
            => Response(null, embed, null);

        protected virtual DiscordCommandResult Response(string content, LocalEmbedBuilder embed, LocalMentionsBuilder mentions = null)
            => Response(new LocalMessageBuilder()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions)
                .Build());

        //protected virtual DiscordCommandResult Response(LocalAttachment attachment)
        //    => Response();

        protected virtual DiscordCommandResult Response(LocalMessage message)
            => new DiscordResponseCommandResult(message)
            {
                Context = Context
            };

        protected virtual DiscordCommandResult Reaction(IEmoji emoji)
            => new DiscordReactionCommandResult(emoji)
            {
                Context = Context
            };

        /// <summary>
        ///     Gets an instance of <see cref="DefaultRestRequestOptions"/> configured with the <see cref="StoppingToken"/>.
        /// </summary>
        protected virtual IRestRequestOptions RequestOptions => new DefaultRestRequestOptions
        {
            CancellationToken = StoppingToken
        };
    }
}