using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase
    {
        protected virtual ValueTask<bool> CheckMessageAsync(IGatewayUserMessage message)
            => new(!message.Author.IsBot);

        /// <summary>
        ///     Creates a <see cref="DiscordCommandContext"/> from the provided parameters.
        /// </summary>
        /// <param name="prefix"> The prefix found in the message. </param>
        /// <param name="message"> The messsage possibly containing the command. </param>
        /// <param name="channel"> The optional cached text channel the message was sent in. </param>
        /// <returns>
        ///     A <see cref="DiscordCommandContext"/> or a <see cref="DiscordGuildCommandContext"/> for guild messages.
        /// </returns>
        protected virtual DiscordCommandContext CreateCommandContext(IPrefix prefix, IGatewayUserMessage message, CachedTextChannel channel)
        {
            var scope = Services.CreateScope();
            var context = message.GuildId != null
                ? new DiscordGuildCommandContext(this, prefix, message, channel, scope)
                : new DiscordCommandContext(this, prefix, message, scope);
            context.Services.GetRequiredService<ICommandContextAccessor>().Context = context;
            return context;
        }

        protected virtual ValueTask<bool> BeforeExecutedAsync(DiscordCommandContext context)
            => new(true);

        protected virtual Task HandleFailedResultAsync(DiscordCommandContext context, FailedResult result)
        {
            if (result is CommandNotFoundResult)
                return Task.CompletedTask;

            return this.SendMessageAsync(context.ChannelId, new LocalMessageBuilder()
                .WithContent(result.FailureReason)
                .Build());
        }
    }
}