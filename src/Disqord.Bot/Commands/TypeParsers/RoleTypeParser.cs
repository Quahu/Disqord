using System;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon.Collections.Synchronized;

namespace Disqord.Bot.Commands.Parsers;

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
    public override ValueTask<ITypeParserResult<IRole>> ParseAsync(IDiscordGuildCommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        if (!context.Bot.CacheProvider.TryGetRoles(context.GuildId, out var roleCache))
            throw new InvalidOperationException($"The {GetType().Name} requires the role cache.");

        var valueSpan = value.Span;
        IRole? foundRole = null;
        if (Snowflake.TryParse(valueSpan, out var id) || Mention.TryParseRole(valueSpan, out id))
        {
            // The value is a mention or an ID.
            foundRole = roleCache.GetValueOrDefault(id);
        }
        else
        {
            // The value is possibly a name.
            foreach (var role in roleCache.Values)
            {
                if (valueSpan.Equals(role.Name, StringComparison.Ordinal))
                {
                    foundRole = role;
                    break;
                }
            }
        }

        if (foundRole != null)
            return Success(new(foundRole));

        return Failure("No role found matching the input.");
    }
}
