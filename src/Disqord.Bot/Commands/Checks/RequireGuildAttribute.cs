using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed within a guild.
///     If the <see cref="Id"/> is specified, then execution is limited to the given guild.
/// </summary>
public class RequireGuildAttribute : DiscordGuildCheckAttribute
{
    public Snowflake? Id { get; }

    public RequireGuildAttribute()
    { }

    public RequireGuildAttribute(ulong id)
    {
        Id = id;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        if (Id == null || context.GuildId == Id)
            return Results.Success;

        return Results.Failure($"This can only be executed in the guild with the ID {Id}.");
    }
}
