using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed within a guild.
/// </summary>
/// <remarks>
///     If <see cref="Id"/> is specified, then execution is limited to the guild with that ID.
/// </remarks>
public class RequireGuildAttribute : DiscordGuildCheckAttribute
{
    /// <summary>
    ///     Gets the ID of the required guild.
    /// </summary>
    public Snowflake? Id { get; }

    /// <summary>
    ///     Instantiates a new <see cref="RequireGuildAttribute"/>
    ///     with no guild ID.
    /// </summary>
    public RequireGuildAttribute()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="RequireGuildAttribute"/>
    ///     with the ID of the required guild.
    /// </summary>
    public RequireGuildAttribute(ulong id)
    {
        Id = id;
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        if (Id == null || context.GuildId == Id)
            return Results.Success;

        return Results.Failure($"This can only be executed in the guild with the ID {Id}.");
    }
}
