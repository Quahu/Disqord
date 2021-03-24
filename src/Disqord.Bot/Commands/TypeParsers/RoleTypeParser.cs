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
        public override ValueTask<TypeParserResult<IRole>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            if (!context.Bot.CacheProvider.TryGetRoles(context.GuildId, out var roles))
                throw new InvalidOperationException($"The {GetType().Name} requires the role cache.");

            IRole role;
            if (Mention.TryParseRole(value, out var id) || Snowflake.TryParse(value, out id))
            {
                // The value is a mention or id.
                role = roles.GetValueOrDefault(id);
            }
            else
            {
                // The value is possibly a name.
                role = roles.Values.FirstOrDefault(x => x.Name == value);
            }

            if (role != null)
                return Success(role);

            return Failure("No role found matching the input.");
        }
    }
}
