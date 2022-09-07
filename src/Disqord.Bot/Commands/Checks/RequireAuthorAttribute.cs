using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by given author.
/// </summary>
public class RequireAuthorAttribute : DiscordCheckAttribute
{
    /// <summary>
    ///     Gets the required ID of the author.
    /// </summary>
    public Snowflake Id { get; }

    /// <summary>
    ///     Instantiates a new <see cref="RequireAuthorAttribute"/>.
    /// </summary>
    /// <param name="id"> The required ID of the author. </param>
    public RequireAuthorAttribute(ulong id)
    {
        Id = id;
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context)
    {
        if (context.Author.Id == Id)
            return Results.Success;

        return Results.Failure($"This can only be executed by the user with the ID {Id}.");
    }
}
