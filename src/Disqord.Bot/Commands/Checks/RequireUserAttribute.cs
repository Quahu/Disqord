using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot
{
    public sealed class RequireUserAttribute : CheckAttribute
    {
        public IReadOnlyList<ulong> Ids { get; }

        public RequireUserAttribute(params ulong[] ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            Ids = ids.ToImmutableArray();
        }

        public override ValueTask<CheckResult> CheckAsync(CommandContext _)
        {
            var context = _ as DiscordCommandContext;
            for (var i = 0; i < Ids.Count; i++)
            {
                if (Ids[i] == context.User.Id)
                    return CheckResult.Successful;
            }

            return CheckResult.Unsuccessful("You are not authorized to execute this.");
        }
    }
}
