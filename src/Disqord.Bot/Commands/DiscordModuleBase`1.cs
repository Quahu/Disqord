using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordModuleBase<T> : ModuleBase<T>
        where T : DiscordCommandContext
    {
        protected virtual DiscordCommandResult Reply(string content, LocalMentionsBuilder mentions = null)
            => Reply(content, null, mentions);

        protected virtual DiscordCommandResult Reply(LocalEmbedBuilder embed)
            => Reply(null, embed, null);

        protected virtual DiscordCommandResult Reply(string content, LocalEmbedBuilder embed, LocalMentionsBuilder mentions = null)
        {
            var builder = new LocalMessageBuilder()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions)
                .WithReply(Context.Message.Id, Context.Message.ChannelId)
                .Build();
            return new DiscordResponseCommandResult(builder);
        }

        protected virtual DiscordCommandResult Response(string content, LocalMentionsBuilder mentions = null)
            => Response(content, null, mentions);

        protected virtual DiscordCommandResult Response(LocalEmbedBuilder embed)
            => Response(null, embed, null);

        protected virtual DiscordCommandResult Response(string content, LocalEmbedBuilder embed, LocalMentionsBuilder mentions = null)
        {
            var builder = new LocalMessageBuilder()
                .WithContent(content)
                .WithEmbed(embed)
                .WithMentions(mentions)
                .Build();
            return new DiscordResponseCommandResult(builder);
        }

        //protected virtual DiscordCommandResult Response(LocalAttachment attachment)
        //    => Response();

        protected virtual DiscordCommandResult Response(LocalMessage message)
            => new DiscordResponseCommandResult(message);

        protected virtual DiscordCommandResult Reaction(IEmoji emoji)
            => new DiscordReactionCommandResult(emoji);
    }
}