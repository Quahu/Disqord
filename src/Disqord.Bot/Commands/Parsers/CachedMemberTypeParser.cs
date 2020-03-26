using System;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedMemberTypeParser : TypeParser<CachedMember>
    {
        public static CachedMemberTypeParser Instance => _instance ?? (_instance = new CachedMemberTypeParser());

        private static CachedMemberTypeParser _instance;

        private CachedMemberTypeParser()
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
                        var nameSpan = valueSpan.Slice(0, value.Length - 5);
                        var discriminatorSpan = valueSpan.Slice(hashIndex + 1);
                        return x.Name.AsSpan().Equals(nameSpan, default)
                            && x.Discriminator.AsSpan().Equals(discriminatorSpan, default);
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