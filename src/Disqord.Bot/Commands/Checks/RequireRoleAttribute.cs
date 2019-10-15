using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireRoleAttribute : GuildOnlyAttribute
    {
        public IReadOnlyList<ulong> Ids { get; }

        public RequireRoleAttribute(params ulong[] ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            Ids = ids.ToImmutableArray();
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var baseResult = base.CheckAsync(_).Result;
            if (!baseResult.IsSuccessful)
                return baseResult;

            var context = _ as DiscordCommandContext;
            for (var i = 0; i < Ids.Count; i++)
            {
                if (!context.Member.Roles.ContainsKey(Ids[i]))
                    return CheckResult.Unsuccessful("You are not authorized to execute this.");
            }

            return CheckResult.Successful;
        }
    }
}
