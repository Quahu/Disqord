using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon.Collections.Synchronized;

namespace Disqord.Bot.Parsers
{
    /// <summary>
    ///     Represents type parsing for the <see cref="IRole"/> type.
    ///     Does <b>not</b> support parsing roles that are not in the cache.
    /// </summary>
    /// <remarks>
    ///     Supports the following inputs, in order:
    ///     <list type="number">
    ///         <item>
    ///             <term> ID </term>
    ///             <description> The ID of the role. </description>
    ///         </item>
    ///         <item>
    ///             <term> Mention </term>
    ///             <description> The mention of the role. </description>
    ///         </item>
    ///         <item>
    ///             <term> Name </term>
    ///             <description> The name of the role. This is case-sensitive. </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class RoleTypeParser : DiscordGuildTypeParser<IRole>
    {
        /// <inheritdoc/>
        public override ValueTask<TypeParserResult<IRole>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            if (!context.Bot.CacheProvider.TryGetRoles(context.GuildId, out var roleCache))
                throw new InvalidOperationException($"The {GetType().Name} requires the role cache.");

            IRole role;
            if (Snowflake.TryParse(value, out var id) || Mention.TryParseRole(value, out id))
            {
                // The value is a mention or id.
                role = roleCache.GetValueOrDefault(id);
            }
            else
            {
                // The value is possibly a name.
                role = roleCache.Values.FirstOrDefault(x => x.Name == value);
            }

            if (role != null)
                return Success(role);

            return Failure("No role found matching the input.");
        }
    }
}
