using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Rest;
using Disqord.Utilities;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public class MemberTypeParser : DiscordGuildTypeParser<IMember>
    {
        private readonly StringComparison _comparison;

        public MemberTypeParser(StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _comparison = comparison;
        }

        public override async ValueTask<TypeParserResult<IMember>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            IMember member;
            if (Mention.TryParseUser(value, out var id) || Snowflake.TryParse(value, out id))
            {
                // The value is a mention or an ID, so we use that for the lookup.
                if (!context.Guild.Members.TryGetValue(id, out member))
                {
                    // This means it's either an invalid ID or the member isn't cached.
                    // But we don't know which one it is, so we have to query the guild.
                    await using (context.BeginYield())
                    {
                        if (context.Bot.GatewayClient.GetShard(context.GuildId).RateLimiter.IsRateLimited())
                        {
                            // Use a REST call if the gateway is rate-limited.
                            // TODO: evacuate the building if everything is rate-limited
                            member = await context.Bot.FetchMemberAsync(context.GuildId, id).ConfigureAwait(false);
                        }
                        else
                        {
                            // Otherwise use member chunking.
                            // TODO: account for the rate-limited check being a race-condition
                            var members = await context.Bot.Chunker.QueryAsync(context.GuildId, new[] { id });
                            member = members.FirstOrDefault();
                        }
                    }
                }
            }
            else
            {
                string name, discriminator;
                var hashIndex = value.LastIndexOf('#');
                if (hashIndex != -1 && hashIndex + 5 == value.Length)
                {
                    // The value is a tag (Name#0000);
                    name = value.Substring(0, value.Length - 5);
                    discriminator = value.Substring(hashIndex + 1);
                }
                else
                {
                    // The value is a name or a nick.
                    name = value;
                    discriminator = null;
                }

                await using (context.BeginYield())
                {
                    // TODO: utilise the REST query endpoint?
                    var members = await context.Bot.Chunker.QueryAsync(context.GuildId, name);
                    // TODO: exact match on name/nick?
                    member = discriminator != null
                        ? members.FirstOrDefault(x => x.Name.Equals(name, _comparison) && x.Discriminator == discriminator)
                        : members.FirstOrDefault(x => x.Name.Equals(name, _comparison) || x.Nick != null && x.Nick.Equals(name, _comparison));
                }
            }

            if (member != null)
                return Success(member);

            return Failure("No member found matching the input.");
        }
    }
}
