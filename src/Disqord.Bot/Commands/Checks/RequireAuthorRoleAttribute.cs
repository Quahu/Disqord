using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed by authors with the given role.
/// </summary>
public class RequireAuthorRoleAttribute : DiscordGuildCheckAttribute
{
    /// <summary>
    ///     Gets the ID of the required role.
    /// </summary>
    public Snowflake Id { get; }

    /// <summary>
    ///     Instantiates a new <see cref="RequireAuthorRoleAttribute"/>.
    /// </summary>
    /// <param name="id"> The ID of the required role. </param>
    public RequireAuthorRoleAttribute(ulong id)
    {
        Id = id;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        var roleIds = context.Author.RoleIds;
        var roleIdCount = roleIds.Count;
        for (var i = 0; i < roleIdCount; i++)
        {
            var roleId = roleIds[i];
            if (roleId == Id)
                return Results.Success;
        }

        return Results.Failure($"This can only be executed by members with the role ID {Id}.");
    }
}
