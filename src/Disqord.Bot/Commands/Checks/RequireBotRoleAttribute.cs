using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the module or command can only be executed if the bot has the given role.
/// </summary>
public class RequireBotRoleAttribute : DiscordGuildCheckAttribute
{
    /// <summary>
    ///     Gets the ID of the required role.
    /// </summary>
    public Snowflake Id { get; }

    /// <summary>
    ///     Instantiates a new <see cref="RequireBotRoleAttribute"/>.
    /// </summary>
    /// <param name="id"> The ID of the required role. </param>
    public RequireBotRoleAttribute(ulong id)
    {
        Id = id;
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context)
    {
        var currentMember = context.Bot.GetCurrentMember(context.GuildId);
        if (currentMember == null)
            Throw.InvalidOperationException($"{nameof(RequireBotRoleAttribute)} requires the current member cached.");

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
