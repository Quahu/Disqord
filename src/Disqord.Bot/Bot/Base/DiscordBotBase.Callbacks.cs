﻿using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
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
        /// <param name="input"> The input possibly containing the command. </param>
        /// <param name="message"> The message possibly containing the command. </param>
        /// <param name="channel"> The optional cached text channel the message was sent in. </param>
        /// <returns>
        ///     A <see cref="DiscordCommandContext"/> or a <see cref="DiscordGuildCommandContext"/> for guild messages.
        /// </returns>
        public virtual DiscordCommandContext CreateCommandContext(IPrefix prefix, string input, IGatewayUserMessage message, CachedMessageGuildChannel channel)
        {
            var scope = Services.CreateScope();
            var context = message.GuildId != null
                ? new DiscordGuildCommandContext(this, prefix, input, message, channel, scope)
                : new DiscordCommandContext(this, prefix, input, message, scope);
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

        protected virtual LocalMessage FormatFailureMessage(DiscordCommandContext context, FailedResult result)
        {
            static string FormatParameter(Parameter parameter)
            {
                string format;
                if (parameter.IsMultiple)
                {
                    format = "{0}[]";
                }
                else
                {
                    format = parameter.IsRemainder
                        ? "{0}…"
                        : "{0}";
                    format = parameter.IsOptional
                        ? $"[{format}]"
                        : $"<{format}>";
                }

                return string.Format(format, parameter.Name);
            }

            var reason = FormatFailureReason(context, result);
            if (reason == null)
                return null;

            var embed = new LocalEmbed()
                .WithDescription(reason)
                .WithColor(0x2F3136);
            if (result is OverloadsFailedResult overloadsFailedResult)
            {
                foreach (var (overload, overloadResult) in overloadsFailedResult.FailedOverloads)
                {
                    var overloadReason = FormatFailureReason(context, overloadResult);
                    if (overloadReason == null)
                        continue;

                    embed.AddField($"Overload: {overload.FullAliases[0]} {string.Join(' ', overload.Parameters.Select(FormatParameter))}", overloadReason);
                }
            }
            else if (context.Command != null)
            {
                embed.WithTitle($"Command: {context.Command.FullAliases[0]} {string.Join(' ', context.Command.Parameters.Select(FormatParameter))}");
            }

            return new LocalMessage()
                .WithEmbeds(embed)
                .WithAllowedMentions(LocalAllowedMentions.None);
        }

        protected virtual ValueTask HandleFailedResultAsync(DiscordCommandContext context, FailedResult result)
        {
            var message = FormatFailureMessage(context, result);
            if (message == null)
                return default;

            return new(this.SendMessageAsync(context.ChannelId, message));
        }

        public virtual async ValueTask<bool> IsOwnerAsync(Snowflake userId)
        {
            var ownerIds = OwnerIds;
            if (ownerIds.Count != 0)
                return ownerIds.Any(x => x == userId);

            var application = await this.FetchCurrentApplicationAsync().ConfigureAwait(false);
            if (application.Team != null)
            {
                OwnerIds = application.Team.Members.Keys.ToReadOnlyList();
                return application.Team.Members.ContainsKey(userId);
            }

            OwnerIds = new[] { application.Owner.Id }.ReadOnly();
            return application.Owner.Id == userId;
        }
    }
}
