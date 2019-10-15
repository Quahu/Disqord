using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Bot
{
    public abstract class DiscordModuleBase : DiscordModuleBase<DiscordCommandContext>
    { }

    public class DiscordModuleBase<TContext> : ModuleBase<TContext>
        where TContext : DiscordCommandContext
    {
        protected virtual Task<RestUserMessage> ReplyAsync(string content = null, bool isTts = false, Embed embed = null,
            RestRequestOptions options = null)
            => Context.Channel.SendMessageAsync(content, isTts, embed, options);

        protected virtual Task<RestUserMessage> ReplyAsync(LocalAttachment attachment, string content = null, bool isTts = false, Embed embed = null,
            RestRequestOptions options = null)
            => Context.Channel.SendMessageAsync(attachment, content, isTts, embed, options);

        protected virtual Task<RestUserMessage> ReplyAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool isTts = false, Embed embed = null,
            RestRequestOptions options = null)
            => Context.Channel.SendMessageAsync(attachments, content, isTts, embed, options);
    }
}
