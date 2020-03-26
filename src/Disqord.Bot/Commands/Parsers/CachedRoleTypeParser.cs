using System;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedRoleTypeParser : TypeParser<CachedRole>
    {
        public static CachedRoleTypeParser Instance => _instance ?? (_instance = new CachedRoleTypeParser());

        private static CachedRoleTypeParser _instance;

        private CachedRoleTypeParser()
        { }

        public override ValueTask<TypeParserResult<CachedRole>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            var Context = (DiscordCommandContext) context;
            if (Context.Guild == null)
                throw new InvalidOperationException("This can only be used in a guild.");

            CachedRole role = null;
            if (Discord.TryParseRoleMention(value, out var id) || Snowflake.TryParse(value, out id))
                Context.Guild.Roles.TryGetValue(id, out role);

            if (role == null)
                role = Context.Guild.Roles.Values.FirstOrDefault(x => x.Name == value);

            return role == null
                ? new TypeParserResult<CachedRole>("No role found matching the input.")
                : new TypeParserResult<CachedRole>(role);
        }
    }
}