using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot.Commands;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class RequireHierarchyBaseAttribute : DiscordGuildParameterCheckAttribute
{
    protected abstract (string Name, IMember Member) GetTarget(IDiscordGuildCommandContext context);

    public override bool CanCheck(IParameter parameter, object? value)
        => value is IMember or IRole;

    public override ValueTask<IResult> CheckAsync(IDiscordGuildCommandContext context, IParameter parameter, object? argument)
    {
        var (targetName, target) = GetTarget(context);
        if (argument is IMember memberArgument)
        {
            if (target.GetHierarchy() > memberArgument.GetHierarchy())
                return Results.Success;
        }
        else
        {
            var roleArgument = argument as IRole;
            if (target.GetHierarchy() > roleArgument!.Position)
                return Results.Success;
        }

        return Results.Failure($"The provided {(argument is IMember ? "member" : "role")} must be below the {targetName} in role hierarchy.");
    }
}
