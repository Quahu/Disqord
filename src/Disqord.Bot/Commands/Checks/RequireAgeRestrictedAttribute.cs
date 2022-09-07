using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed in age restricted guild channels.
/// </summary>
public class RequireAgeRestrictedAttribute : DiscordGuildCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireAgeRestrictedAttribute"/>.
    /// </summary>
    public RequireAgeRestrictedAttribute()
    { }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        var channel = context.Bot.GetChannel(context.GuildId, context.ChannelId) as IGuildChannel;
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
