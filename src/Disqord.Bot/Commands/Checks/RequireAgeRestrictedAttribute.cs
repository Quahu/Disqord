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
///     If a nested (i.e. <see cref="SlashGroupAttribute"/>) application command is a decorated with this attribute
///     the entire module will be age-restricted.
/// </remarks>
public class RequireAgeRestrictedAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireAgeRestrictedAttribute"/>.
    /// </summary>
    public RequireAgeRestrictedAttribute()
    { }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context is IDiscordApplicationCommandContext)
        {
            // Handled by Discord.
            return Results.Success;
        }

        if (context.GuildId == null)
            return Results.Failure("This can only be executed within a guild.");

        var channel = context.Bot.GetChannel(context.GuildId.Value, context.ChannelId) as IGuildChannel;
        if (channel == null)
            Throw.InvalidOperationException($"{nameof(RequireAgeRestrictedAttribute)} requires the context channel cached.");

        var isAgeRestricted = channel switch
        {
            IAgeRestrictableChannel ageRestrictableChannel => ageRestrictableChannel.IsAgeRestricted,
            IThreadChannel threadChannel => threadChannel.GetChannel()?.IsAgeRestricted ?? false,
            _ => false
        };

        if (isAgeRestricted)
            return Results.Success;

        return Results.Failure("This can only be executed in an age-restricted channel.");
    }
}
