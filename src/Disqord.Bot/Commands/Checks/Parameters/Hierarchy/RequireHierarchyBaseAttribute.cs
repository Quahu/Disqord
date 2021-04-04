using System;
using System.ComponentModel;
using System.Linq;
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
            if (argument is IMember member)
            {
                if (GetHierarchy(context.Guild, target) > GetHierarchy(context.Guild, member))
                    return Success();
            }
            else
            {
                var role = argument as IRole;
                if (GetHierarchy(context.Guild, target) > role.Position)
                    return Success();
            }

            return Failure($"The provided {(argument is IMember ? "member" : "role")} must be below the {targetName} in role hierarchy.");
        }

        public static int GetHierarchy(IGuild guild, IMember member)
        {
            if (guild.OwnerId == member.Id)
                return int.MaxValue;

            // TODO: account for broken positions?
            var roles = member.GetRoles();
            return roles.Count != 0
                ? roles.Values.Max(x => x.Position)
                : 0;
        }
    }
}
