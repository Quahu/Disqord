using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed in the given channel.
/// </summary>
public class RequireChannelAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Gets the ID of the required channel.
    /// </summary>
    public Snowflake Id { get; }

    /// <summary>
    ///     Instantiates a new <see cref="RequireChannelAttribute"/>.
    /// </summary>
    /// <param name="id"> The ID of the required channel. </param>
    public RequireChannelAttribute(ulong id)
    {
        Id = id;
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context.ChannelId == Id)
            return Results.Success;

        return Results.Failure($"This can only be executed in the channel with the ID {Id}.");
    }
}
