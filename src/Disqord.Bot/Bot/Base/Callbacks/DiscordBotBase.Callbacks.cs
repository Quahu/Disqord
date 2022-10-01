using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Bot.Commands.Interaction;
using Disqord.Bot.Commands.Text;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;
using Qmmands.Text;
using Qommon;
using Qommon.Collections.ReadOnly;
using Qommon.Disposal;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    /// <summary>
    ///     Sets the context of the <see cref="ICommandContextAccessor"/>.
    /// </summary>
    /// <param name="context"> The execution context. </param>
    protected internal virtual void SetAccessorContext(IDiscordCommandContext context)
    {
        var accessor = context.Services.GetService<ICommandContextAccessor>();
        if (accessor != null)
            accessor.Context = context;
    }

    /// <summary>
    ///     Invoked pre-execution, right before the command itself is executed.
    /// </summary>
    /// <remarks>
    ///     Returning <see langword="false"/> prevents further execution.
    /// </remarks>
    /// <param name="context"> The execution context. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the work where the result indicates whether execution should proceed.
    /// </returns>
    protected internal virtual ValueTask<IResult> OnBeforeExecuted(IDiscordCommandContext context)
    {
        return new(Results.Success);
    }

    /// <summary>
    ///     Invoked post-execution, before <see cref="OnCommandResult"/>
    ///     and <see cref="OnFailedResult"/> are executed.
    /// </summary>
    /// <remarks>
    ///     Returning <see langword="false"/> prevents further execution.
    /// </remarks>
    /// <param name="context"> The execution context. </param>
    /// <param name="result"> The result of the execution. <see langword="null"/> if the command executed did not return a result. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work where the result indicates whether the appropriate handle method should be executed.
    /// </returns>
    protected virtual ValueTask<bool> OnAfterExecuted(IDiscordCommandContext context, IResult result)
    {
        return new(true);
    }

    /// <summary>
    ///     Invoked post-execution, defines the logic for handling command results.
    /// </summary>
    /// <param name="context"> The execution context. </param>
    /// <param name="result"> The result to handle. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual async ValueTask OnCommandResult(IDiscordCommandContext context, IDiscordCommandResult result)
    {
        await using (RuntimeDisposal.WrapAsync(result).ConfigureAwait(false))
        {
            await result.ExecuteAsync(StoppingToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    ///     Creates a failure message for the given context.
    /// </summary>
    /// <remarks>
    ///     By default this returns:
    ///     <list type="bullet">
    ///         <item>
    ///             <term> <see cref="LocalInteractionMessageResponse"/> </term>
    ///             <description> for <see cref="IDiscordInteractionCommandContext"/> </description>
    ///         </item>
    ///         <item>
    ///             <term> <see cref="LocalMessage"/> </term>
    ///             <description> for <see cref="IDiscordTextCommandContext"/> </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    /// <param name="context"> The execution context. </param>
    /// <returns>
    ///     A <see cref="LocalMessageBase"/> that will be used to send the failure reason.
    /// </returns>
    protected virtual LocalMessageBase? CreateFailureMessage(IDiscordCommandContext context)
    {
        LocalMessageBase message;
        if (context is IDiscordInteractionCommandContext interactionContext)
        {
            var response = interactionContext.Interaction.Response();
            if (response.HasResponded && response.ResponseType is InteractionResponseType.DeferredChannelMessage or InteractionResponseType.DeferredMessageUpdate
                or InteractionResponseType.MessageUpdate)
            {
                // Ignore interactions that have been deferred or would modify an existing message.
                return null;
            }

            message = new LocalInteractionMessageResponse()
                .WithIsEphemeral();
        }
        else
        {
            message = new LocalMessage();
        }

        message.WithAllowedMentions(LocalAllowedMentions.None);
        return message;
    }

    /// <summary>
    ///     Translates a failed result into a <see cref="string"/>.
    /// </summary>
    /// <param name="context"> The execution context. </param>
    /// <param name="result"> The result to format. </param>
    /// <returns>
    ///     A <see cref="string"/> representing the failure reason.
    /// </returns>
    protected virtual string? FormatFailureReason(IDiscordCommandContext context, IResult result)
    {
        return result switch
        {
            CommandNotFoundResult => null,
            TypeParseFailedResult typeParseFailedResult => $"Type parse failed for parameter '{typeParseFailedResult.Parameter.Name}':\n• {typeParseFailedResult.FailureReason}",
            ChecksFailedResult checksFailedResult => string.Join('\n', checksFailedResult.FailedChecks.Select(x => $"• {x.Value.FailureReason}")),
            ParameterChecksFailedResult parameterChecksFailedResult => $"Checks failed for parameter '{parameterChecksFailedResult.Parameter.Name}':\n"
                + string.Join('\n', parameterChecksFailedResult.FailedChecks.Select(x => $"• {x.Value.FailureReason}")),
            _ => result.FailureReason
        };
    }

    /// <summary>
    ///     Formats a failed result into a <see cref="LocalMessageBase"/>.
    /// </summary>
    /// <param name="context"> The execution context. </param>
    /// <param name="message"> The message to format. </param>
    /// <param name="result"> The result to format. </param>
    /// <returns>
    ///     A <see cref="bool"/> indicating whether the message should be sent.
    /// </returns>
    protected virtual bool FormatFailureMessage(IDiscordCommandContext context, LocalMessageBase message, IResult result)
    {
        static string FormatParameter(IParameter parameter)
        {
            var typeInformation = parameter.GetTypeInformation();
            var format = "{0}";
            if (typeInformation.IsEnumerable)
            {
                format = "{0}[]";
            }
            else if (parameter is IPositionalParameter positionalParameter && positionalParameter.IsRemainder)
            {
                format = "{0}…";
            }

            format = typeInformation.IsOptional
                ? $"[{format}]"
                : $"<{format}>";

            return string.Format(format, parameter.Name);
        }

        var reason = FormatFailureReason(context, result);
        if (reason == null)
            return false;

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

                embed.AddField($"Overload: {overload.Name} {string.Join(' ', overload.Parameters.Select(FormatParameter))}", overloadReason);
            }
        }
        else if (context.Command != null)
        {
            embed.WithTitle($"Command: {context.Command.Name} {string.Join(' ', context.Command.Parameters.Select(FormatParameter))}");
        }

        message.AddEmbed(embed);
        return true;
    }

    /// <summary>
    ///     Sends a formatted failure message.
    /// </summary>
    /// <param name="context"> The execution context. </param>
    /// <param name="message"> The message to send. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask SendFailureMessageAsync(IDiscordCommandContext context, LocalMessageBase message)
    {
        [DoesNotReturn]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static ValueTask ThrowTypeException(object value, [CallerArgumentExpression("value")] string? what = null)
        {
            return Throw.InvalidOperationException<ValueTask>($"Unknown {what} type '{value.GetType()}', override {nameof(SendFailureMessageAsync)} to define behavior for it.");
        }

        Task task;
        if (context is IDiscordInteractionCommandContext interactionContext)
        {
            if (message is LocalInteractionMessageResponse localInteractionMessageResponse)
            {
                task = interactionContext.SendMessageAsync(localInteractionMessageResponse, false, cancellationToken: context.CancellationToken);
            }
            else
            {
                return ThrowTypeException(message);
            }
        }
        else if (context is IDiscordTextCommandContext)
        {
            if (message is LocalMessage localMessage)
            {
                task = this.SendMessageAsync(context.ChannelId, localMessage, cancellationToken: context.CancellationToken);
            }
            else
            {
                return ThrowTypeException(message);
            }
        }
        else
        {
            return ThrowTypeException(context);
        }

        return new(task);
    }

    /// <summary>
    ///     Invoked post-execution, defines the logic for handling failed results.
    /// </summary>
    /// <param name="context"> The execution context. </param>
    /// <param name="result"> The result to handle. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the work.
    /// </returns>
    protected virtual ValueTask OnFailedResult(IDiscordCommandContext context, IResult result)
    {
        if (context is IDiscordInteractionCommandContext interactionContext)
        {
            if (interactionContext.Interaction is IAutoCompleteInteraction)
            {
                // Ignore auto-complete interaction failures as those cannot be responded to.
                // This is passed through so that the user can log and debug failures.
                return default;
            }
        }

        var message = CreateFailureMessage(context);
        if (message == null)
            return default;

        if (!FormatFailureMessage(context, message, result))
            return default;

        return SendFailureMessageAsync(context, message);
    }

    /// <summary>
    ///     Checks if the user of the provided ID is an owner of this bot.
    /// </summary>
    /// <param name="userId"> The ID of the user to check. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the work where the result indicates whether the user is an owner.
    /// </returns>
    public virtual async ValueTask<bool> IsOwnerAsync(Snowflake userId)
    {
        var ownerIds = OwnerIds;
        var ownerIdCount = ownerIds.Count;
        if (ownerIdCount != 0)
        {
            for (var i = 0; i < ownerIdCount; i++)
            {
                var ownerId = ownerIds[i];
                if (ownerId == userId)
                    return true;
            }

            return false;
        }

        var application = _currentApplication ??= await this.FetchCurrentApplicationAsync(cancellationToken: StoppingToken).ConfigureAwait(false);
        if (application.Team != null)
        {
            OwnerIds = application.Team.Members.Keys.ToReadOnlyList();
            return application.Team.Members.ContainsKey(userId);
        }

        OwnerIds = new[] { application.Owner!.Id }.ReadOnly();
        return application.Owner.Id == userId;
    }
}
