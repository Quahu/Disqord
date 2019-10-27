using System;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedMemberParser : TypeParser<CachedMember>
    {
        public static CachedMemberParser Instance => _instance ?? (_instance = new CachedMemberParser());

        private static CachedMemberParser _instance;

        private CachedMemberParser()
        { }

        public override ValueTask<TypeParserResult<CachedMember>> ParseAsync(Parameter parameter, string value, CommandContext _)
        {
            var context = (DiscordCommandContext) _;
            if (context.Guild == null)
                throw new InvalidOperationException("This can only be used in a guild.");

            CachedMember member = null;
            if (Discord.TryParseUserMention(value, out var id) || Snowflake.TryParse(value, out id))
                context.Guild.Members.TryGetValue(id, out member);

            var values = context.Guild.Members.Values;
            if (member == null)
            {
                var hashIndex = value.LastIndexOf('#');
                if (hashIndex != -1 && hashIndex + 5 == value.Length)
                {
                    member = values.FirstOrDefault(x =>
                    {
                        var valueSpan = value.AsSpan();
                        return x.Name.AsSpan().Equals(valueSpan.Slice(0, value.Length - 5), StringComparison.Ordinal) &&
                            x.Discriminator == valueSpan.Slice(hashIndex + 1);
                    });
                }
            }

            if (member == null)
            {
                // TODO custom result type returning the members?
                var matchingMembers = values.Where(x => x.Name == value || x.Nick == value).ToArray();
                if (matchingMembers.Length > 1)
                    return TypeParserResult<CachedMember>.Unsuccessful("Multiple matches found. Mention the member, use their tag or their ID.");

                if (matchingMembers.Length == 1)
                    member = matchingMembers[0];
            }

            return member == null
                ? TypeParserResult<CachedMember>.Unsuccessful("No member found matching the input.")
                : TypeParserResult<CachedMember>.Successful(member);
        }
    }
}