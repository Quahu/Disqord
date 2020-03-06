using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordModuleBase : DiscordModuleBase<DiscordCommandContext>
    { }

    public class DiscordModuleBase<TContext> : ModuleBase<TContext>
        where TContext : DiscordCommandContext
    {
        protected override ValueTask BeforeExecutedAsync()
        {
#if DEBUG
            Context.Bot.Log(LogMessageSeverity.Information, $"Executing {Context.Command} by {Context.User} in {FormatChannel()}.");
#endif
            return default;
        }

        protected override ValueTask AfterExecutedAsync()
        {
#if DEBUG
            Context.Bot.Log(LogMessageSeverity.Information, $"Executed {Context.Command} by {Context.User} in {FormatChannel()}.");
#endif
            return default;
        }

#if DEBUG
        private string FormatChannel()
            => Context.Guild != null
                ? $"{Context.Channel} ({Context.Channel.Id}); Guild: {Context.Guild} ({Context.Guild.Id})"
                : $"{Context.Channel} ({Context.Channel.Id}).";
#endif

        protected virtual Task<RestUserMessage> ReplyAsync(string content = null, bool isTts = false, LocalEmbed embed = null, LocalMentions mentions = null,
            RestRequestOptions options = null)
            => Context.Channel.SendMessageAsync(content, isTts, embed, mentions, options);

        protected virtual Task<RestUserMessage> ReplyAsync(LocalAttachment attachment, string content = null, bool isTts = false, LocalEmbed embed = null, LocalMentions mentions = null,
            RestRequestOptions options = null)
            => Context.Channel.SendMessageAsync(attachment, content, isTts, embed, mentions, options);

        protected virtual Task<RestUserMessage> ReplyAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTts = false, LocalEmbed embed = null, LocalMentions mentions = null,
            RestRequestOptions options = null)
            => Context.Channel.SendMessageAsync(attachments, content, isTts, embed, mentions, options);
    }
}
