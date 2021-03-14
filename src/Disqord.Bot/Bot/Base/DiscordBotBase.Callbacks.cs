using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace Disqord.Bot
{
    public abstract partial class DiscordBotBase
    {
        /// <summary>
        ///     Checks if the received message is valid.
        ///     By default ensures the message author is not a bot. 
        /// </summary>
        /// <param name="message"> The message to check. </param>
        /// <returns>
        ///     A <see cref="ValueTask{TResult}"/> where the result indicates whether the message is valid.
        /// </returns>
        protected virtual ValueTask<bool> CheckMessageAsync(IGatewayUserMessage message)
            => new(!message.Author.IsBot);

        /// <summary>
        ///     Creates a <see cref="DiscordCommandContext"/> from the provided parameters.
        /// </summary>
        /// <param name="prefix"> The prefix found in the message. </param>
        /// <param name="message"> The message possibly containing the command. </param>
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

        protected virtual string FormatFailureReason(DiscordCommandContext context, FailedResult result)
            => result switch
            {
                CommandNotFoundResult => null,
                TypeParseFailedResult typeParseFailedResult => $"Type parse failed for parameter '{typeParseFailedResult.Parameter}':\n• {typeParseFailedResult.FailureReason}",
                ChecksFailedResult checksFailedResult => string.Join('\n', checksFailedResult.FailedChecks.Select(x => $"• {x.Result.FailureReason}")),
                ParameterChecksFailedResult parameterChecksFailedResult => $"Checks failed for parameter '{parameterChecksFailedResult.Parameter}':\n"
                    + string.Join('\n', parameterChecksFailedResult.FailedChecks.Select(x => $"• {x.Result.FailureReason}")),
                _ => result.FailureReason
            };

        protected virtual LocalMessageBuilder FormatFailureMessage(DiscordCommandContext context, FailedResult result)
        {
            var reason = FormatFailureReason(context, result);
            if (reason == null)
                return null;

            var embed = new LocalEmbedBuilder()
                .WithDescription(reason)
                .WithColor(0x2F3136);
            if (result is OverloadsFailedResult overloadsFailedResult)
            {
                foreach (var (overload, overloadResult) in overloadsFailedResult.FailedOverloads)
                {
                    var overloadReason = FormatFailureReason(context, overloadResult);
                    if (overloadReason == null)
                        continue;

                    embed.AddField($"{overload} {string.Join(' ', overload.Parameters)}", overloadReason);
                }
            }
            else if (context.Command != null)
            {
                embed.WithTitle($"{context.Command} {string.Join(' ', context.Command.Parameters)}");
            }

            return new LocalMessageBuilder()
                .WithEmbed(embed);
        }

        protected virtual Task HandleFailedResultAsync(DiscordCommandContext context, FailedResult result)
        {
            var message = FormatFailureMessage(context, result)?.Build();
            if (message == null)
                return Task.CompletedTask;

            return this.SendMessageAsync(context.ChannelId, message);
        }
    }
}
