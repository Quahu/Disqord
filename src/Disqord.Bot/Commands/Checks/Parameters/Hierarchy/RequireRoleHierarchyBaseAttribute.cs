using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Represents the base attribute for hierarchy-based checks
///     for <see cref="IMember"/> and <see cref="IRole"/> parameters.
/// </summary>
public abstract class RequireRoleHierarchyBaseAttribute : DiscordGuildParameterCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireRoleHierarchyBaseAttribute"/>.
    /// </summary>
    protected RequireRoleHierarchyBaseAttribute()
    { }

    /// <summary>
    ///     Gets the name and target tuple for the check logic.
    /// </summary>
    /// <param name="context"> The command context. </param>
    /// <returns>
    ///     The name and target tuple.
    /// </returns>
    protected abstract (string Name, IMember Member) GetTarget(IDiscordGuildCommandContext context);

    /// <inheritdoc/>
    public override bool CanCheck(IParameter parameter, object? value)
    {
        return value is IMember or IRole;
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context, IParameter parameter, object? argument)
    {
        var (targetName, target) = GetTarget(context);
        var guild = context.Bot.GetGuild(context.GuildId);
        if (guild != null && target.Id == guild.OwnerId)
            return Results.Success;

        if (argument is IMember memberArgument)
        {
            if (Comparers.Roles.Compare(target.GetHighestRole(), memberArgument.GetHighestRole()) > 0)
                return Results.Success;
        }
        else
        {
            var roleArgument = argument as IRole;
            if (Comparers.Roles.Compare(target.GetHighestRole(), roleArgument) > 0)
                return Results.Success;
        }

        return Results.Failure($"The provided {(argument is IMember ? "member" : "role")} must be below the {targetName} in role hierarchy.");
    }
}
