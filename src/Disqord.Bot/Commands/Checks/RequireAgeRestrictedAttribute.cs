using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed in age restricted guild channels.
/// </summary>
public class RequireAgeRestrictedAttribute : DiscordGuildCheckAttribute
{
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        if (context.Bot.GetChannel(context.GuildId, context.ChannelId) is not IGuildChannel channel)
            throw new InvalidOperationException($"{nameof(RequireAgeRestrictedAttribute)} requires the context channel.");

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
