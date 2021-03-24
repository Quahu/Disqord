using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Utilities;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public class RoleTypeParser : DiscordGuildTypeParser<IRole>
    {
        private readonly StringComparison _comparison;

        public RoleTypeParser(StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _comparison = comparison;
        }

        public override ValueTask<TypeParserResult<IRole>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            if (!context.Bot.CacheProvider.TryGetRoles(context.GuildId, out var cache))
                throw new InvalidOperationException($"The {GetType().Name} requires the role cache.");

            IRole role = null;
            if (Mention.TryParseChannel(value, out var id) || Snowflake.TryParse(value, out id))
            {
                role = cache.GetValueOrDefault(id);
            }
            else
            {
                role = cache.Values.FirstOrDefault(x => x.Name.Equals(value, _comparison));
            }

            if (role != null)
                return Success(role);

            return Failure("No role found matching the input.");
        }
    }
}
