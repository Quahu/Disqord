using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireAuthorHierarchyAttribute : DiscordGuildParameterCheckAttribute
    {
        public override bool CheckType(Type type)
            => typeof(IMember).IsAssignableFrom(type);

        public override ValueTask<CheckResult> CheckAsync(object argument, DiscordGuildCommandContext context)
        {
            if (context.Guild == null)
                throw new InvalidOperationException($"{GetType().Name} requires a non-null guild.");

            var author = context.Author;
            var member = argument as IMember;
            if (author.Id != member.Id
                && context.Guild.OwnerId != author.Id
                && context.Guild.OwnerId != member.Id)
            {
                // TODO: account for broken positions?
                var memberRoles = member.GetRoles();
                var memberHierarchy = memberRoles.Count != 0
                    ? memberRoles.Values.Max(x => x.Position)
                    : 0;
                var authorRoles = author.GetRoles();
                var authorHierarchy = authorRoles.Count != 0
                    ? authorRoles.Values.Max(x => x.Position)
                    : 0;

                if (authorHierarchy > memberHierarchy)
                    return Success();
            }

            return Failure("The provided member must be below the author in role hierarchy.");
        }
    }
}
