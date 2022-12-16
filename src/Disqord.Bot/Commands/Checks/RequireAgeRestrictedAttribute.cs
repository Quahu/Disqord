using System.Threading.Tasks;
using Disqord.Bot.Commands.Application;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed in age-restricted guild channels.
/// </summary>
/// <remarks>
///     For application commands this also allows the command to be used
///     in direct channels with the application.
///     <br/>
///     If a nested (i.e. <see cref="SlashGroupAttribute"/>) application command is decorated with this attribute
///     the entire module will be age-restricted.
/// </remarks>
public class RequireAgeRestrictedAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireAgeRestrictedAttribute"/>.
    /// </summary>
    public RequireAgeRestrictedAttribute()
    { }

    private static bool IsAgeRestricted(IGuildChannel channel)
    {
        return channel switch
        {
            IAgeRestrictableChannel ageRestrictableChannel => ageRestrictableChannel.IsAgeRestricted,
            IThreadChannel threadChannel => threadChannel.GetChannel()?.IsAgeRestricted ?? false,
            _ => false
        };
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        IGuildChannel? channel;
        if (context is IDiscordApplicationCommandContext)
        {
            // Handled by Discord, but we'll do some checking.
            if (context.GuildId == null)
                return Results.Success;

            channel = context.Bot.GetChannel(context.GuildId.Value, context.ChannelId);
            if (channel == null)
                return Results.Success;
        }
        else
        {
            if (context.GuildId == null)
                return Results.Failure("This can only be executed within a guild.");

            channel = context.Bot.GetChannel(context.GuildId.Value, context.ChannelId);
            if (channel == null)
                Throw.InvalidOperationException($"{nameof(RequireAgeRestrictedAttribute)} requires the context channel cached.");
        }

        var isAgeRestricted = IsAgeRestricted(channel);
        if (isAgeRestricted)
            return Results.Success;

        return Results.Failure("This can only be executed in an age-restricted channel.");
    }
}
