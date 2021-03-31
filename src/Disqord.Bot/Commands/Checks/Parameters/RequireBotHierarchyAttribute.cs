﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;

namespace Disqord.Bot
{
    public class RequireBotHierarchyAttribute : DiscordGuildParameterCheckAttribute
    {
        public RequireBotHierarchyAttribute()
            : base(x => typeof(IMember).IsAssignableFrom(x))
        { }

        public override ValueTask<CheckResult> CheckAsync(object argument, DiscordGuildCommandContext context)
        {
            if (context.Guild == null)
                throw new InvalidOperationException($"{GetType().Name} requires a non-null guild.");

            var bot = context.CurrentMember;
            var member = argument as IMember;
            if (bot.Id != member.Id
                && context.Guild.OwnerId != bot.Id
                && context.Guild.OwnerId != member.Id)
            {
                // TODO: account for broken positions?
                var memberRoles = member.GetRoles();
                var memberHierarchy = memberRoles.Count != 0
                    ? memberRoles.Values.Max(x => x.Position)
                    : 0;
                var botRoles = bot.GetRoles();
                var botHierarchy = botRoles.Count != 0
                    ? botRoles.Values.Max(x => x.Position)
                    : 0;

                if (botHierarchy > memberHierarchy)
                    return Success();
            }

            return Failure("The provided member must be below the bot in role hierarchy.");
        }
    }
}