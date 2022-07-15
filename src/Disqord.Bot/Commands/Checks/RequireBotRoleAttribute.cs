using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has the given role.
/// </summary>
public class RequireBotRoleAttribute : DiscordGuildCheckAttribute
{
    public Snowflake Id { get; }

    public RequireBotRoleAttribute(ulong id)
    {
        Id = id;
    }

    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        var currentMember = context.Bot.GetMember(context.GuildId, context.Bot.CurrentUser.Id);
        var roleIds = currentMember.RoleIds;
        var roleIdCount = roleIds.Count;
        for (var i = 0; i < roleIdCount; i++)
        {
            var roleId = roleIds[i];
            if (roleId == Id)
                return Results.Success;
        }

        return Results.Failure($"The bot requires the role with ID {Id}.");
    }
}
