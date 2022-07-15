using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed in the given channel.
/// </summary>
public class RequireChannelAttribute : DiscordCheckAttribute
{
    public Snowflake Id { get; }

    public RequireChannelAttribute(ulong id)
    {
        Id = id;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context.ChannelId == Id)
            return Results.Success;

        return Results.Failure($"This can only be executed in the channel with the ID {Id}.");
    }
}
