using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Rest;
using Qmmands;

namespace Disqord.Bot.Commands.Parsers;

/// <summary>
///     Represents type parsing for the <see cref="IMember"/> type.
///     Supports parsing members that are not in the cache.
/// </summary>
/// <remarks>
///     Supports the following inputs, in order:
///     <list type="number">
///         <item>
///             <term> ID </term>
///             <description> The ID of the member. </description>
///         </item>
///         <item>
///             <term> Mention </term>
///             <description> The mention of the member. </description>
///         </item>
///         <item>
///             <term> Tag </term>
///             <description> The tag of the member. This is case-sensitive. </description>
///         </item>
///         <item>
///             <term> Name / Nick </term>
///             <description> The name or nick of the member. This is case-sensitive. </description>
///         </item>
///     </list>
/// </remarks>
public class MemberTypeParser : DiscordGuildTypeParser<IMember>
{
    /// <inheritdoc/>
    public override async ValueTask<ITypeParserResult<IMember>> ParseAsync(IDiscordGuildCommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        if (!context.Bot.CacheProvider.TryGetMembers(context.GuildId, out var memberCache))
            throw new InvalidOperationException($"The {GetType().Name} requires the member cache.");

        static bool TryParseUserId(ReadOnlySpan<char> value, out Snowflake id)
            => Snowflake.TryParse(value, out id) || Mention.TryParseUser(value, out id);

        IMember? member;
        if (TryParseUserId(value.Span, out var id))
        {
            // The value is a mention or an ID.
            // We look up the cache first.
            if (!memberCache.TryGetValue(id, out var cachedMember))
            {
                // This means it's either an invalid ID or the member isn't cached.
                // We don't know which one it is, so we have to query the guild.

                // Check if the gateway is/will be rate-limited.
                if (context.Bot.ApiClient.GetShard(context.GuildId)!.RateLimiter.GetRemainingRequests() < 3)
                {
                    // Use a REST call instead.
                    member = await context.Bot.FetchMemberAsync(context.GuildId, id).ConfigureAwait(false);
                }
                else
                {
                    // Otherwise use gateway member chunking.
                    var members = await context.Bot.Chunker.QueryAsync(context.GuildId, new[] { id }).ConfigureAwait(false);
                    member = members.GetValueOrDefault(id);
                }
            }
            else
            {
                // Have to assign the `out var cachedMember` like this.
                member = cachedMember;
            }
        }
        else
        {
            static (string, string?) ParseTag(ReadOnlyMemory<char> value)
            {
                string name;
                string? discriminator;
                var valueSpan = value.Span;
                var hashIndex = valueSpan.LastIndexOf('#');
                if (hashIndex != -1 && hashIndex + 5 == value.Length)
                {
                    // The value is a tag (Name#0000);
                    name = new string(valueSpan.Slice(0, value.Length - 5));
                    discriminator = new string(valueSpan.Slice(hashIndex + 1));
                }
                else
                {
                    // The value is possibly a name or a nick.
                    name = value.ToString();
                    discriminator = null;
                }

                return (name, discriminator);
            }

            // The value is possibly a tag, name, or nick.
            // So let's check for a '#', indicating a tag.
            var (name, discriminator) = ParseTag(value);

            // This method checks checks for tag or name/nick accordingly and is reused below.
            static IMember? FindMember(IEnumerable<IMember> members, string name, string? discriminator)
            {
                if (discriminator != null)
                {
                    // Checks for tag, e.g. Clyde#0001.
                    return members.FirstOrDefault(x => x.Name == name && x.Discriminator == discriminator);
                }

                // Checks for name and then nick.
                return members.FirstOrDefault(x => x.Name == name) ?? members.FirstOrDefault(x => x.Nick == name);
            }

            member = FindMember(memberCache.Values, name, discriminator);
            if (member == null)
            {
                // This means it's either an invalid input or the member isn't cached.

                // We don't know which one it is, so we have to query the guild.
                // Check if the gateway is/will be rate-limited.
                // TODO: swap these two around?
                IEnumerable<IMember> members;
                if (context.Bot.ApiClient.GetShard(context.GuildId)!.RateLimiter.GetRemainingRequests() < 3)
                {
                    members = await context.Bot.SearchMembersAsync(context.GuildId, name).ConfigureAwait(false);
                }
                else
                {
                    members = (await context.Bot.Chunker.QueryAsync(context.GuildId, name).ConfigureAwait(false)).Values;
                }

                member = FindMember(members, name, discriminator);
            }
        }

        if (member != null)
            return Success(new(member));

        return Failure("No member found matching the input.");
    }
}
