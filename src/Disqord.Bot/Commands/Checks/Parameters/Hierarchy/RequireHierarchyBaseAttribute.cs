using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class RequireHierarchyBaseAttribute : DiscordGuildParameterCheckAttribute
    {
        protected abstract (string Name, IMember Member) GetTarget(DiscordGuildCommandContext context);

        public override bool CheckType(Type type)
            => typeof(IMember).IsAssignableFrom(type) || typeof(IRole).IsAssignableFrom(type);

        public override ValueTask<CheckResult> CheckAsync(object argument, DiscordGuildCommandContext context)
        {
            if (context.Guild == null)
                throw new InvalidOperationException($"{GetType().Name} requires a non-null guild.");

            var (targetName, target) = GetTarget(context);
            if (argument is IMember memberArgument)
            {
                if (target.GetHierarchy(context.Guild) > memberArgument.GetHierarchy(context.Guild))
                    return Success();
            }
            else
            {
                var roleArgument = argument as IRole;
                if (target.GetHierarchy(context.Guild) > roleArgument.Position)
                    return Success();
            }

            return Failure($"The provided {(argument is IMember ? "member" : "role")} must be below the {targetName} in role hierarchy.");
        }
    }
}
