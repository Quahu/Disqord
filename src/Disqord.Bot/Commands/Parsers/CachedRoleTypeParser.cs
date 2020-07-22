using System;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedRoleTypeParser : TypeParser<CachedRole>
    {
        private readonly StringComparison _comparison;

        public CachedRoleTypeParser(StringComparison comparison = default)
        {
            _comparison = comparison;
        }

        public override ValueTask<TypeParserResult<CachedRole>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            var Context = (DiscordCommandContext) context;
            if (Context.Guild == null)
                throw new InvalidOperationException("This can only be executed in a guild.");

            CachedRole role = null;
            if (Discord.TryParseRoleMention(value, out var id) || Snowflake.TryParse(value, out id))
                Context.Guild.Roles.TryGetValue(id, out role);

            if (role == null)
                role = Context.Guild.Roles.Values.FirstOrDefault(x => x.Name.Equals(value, _comparison));

            return role == null
                ? new TypeParserResult<CachedRole>("No role found matching the input.")
                : new TypeParserResult<CachedRole>(role);
        }
    }
}