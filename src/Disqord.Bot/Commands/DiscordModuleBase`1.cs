using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordModuleBase<T> : ModuleBase<T>
        where T : DiscordCommandContext
    {
        protected virtual DiscordCommandResult Response(string content, LocalMentionsBuilder mentions = null)
            => Response(content, null, mentions);

        protected virtual DiscordCommandResult Response(LocalEmbedBuilder embed)
            => Response(null, embed);

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