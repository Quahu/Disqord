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
        if (argument is IMember memberArgument)
        {
            if (target.CalculateRoleHierarchy() > memberArgument.CalculateRoleHierarchy())
                return Results.Success;
        }
        else
        {
            var roleArgument = argument as IRole;
            if (target.CalculateRoleHierarchy() > roleArgument!.Position)
                return Results.Success;
        }

        return Results.Failure($"The provided {(argument is IMember ? "member" : "role")} must be below the {targetName} in role hierarchy.");
    }
}
