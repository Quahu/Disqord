using System;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedMemberTypeParser : TypeParser<CachedMember>
    {
        private readonly StringComparison _comparison;

        public CachedMemberTypeParser(StringComparison comparison = default)
        {
            _comparison = comparison;
        }

        public override ValueTask<TypeParserResult<CachedMember>> ParseAsync(Parameter parameter, string value, CommandContext _)
        {
            var context = (DiscordCommandContext) _;
            if (context.Guild == null)
                throw new InvalidOperationException("This can only be executed in a guild.");

            CachedMember member = null;
            if (Discord.TryParseUserMention(value, out var id) || Snowflake.TryParse(value, out id))
                context.Guild.Members.TryGetValue(id, out member);

            if (member == null)
            {
                var members = context.Guild.Members.Values;
                var hashIndex = value.LastIndexOf('#');
                if (hashIndex != -1 && hashIndex + 5 == value.Length)
                {
                    member = members.FirstOrDefault(x =>
                    {
                        var valueSpan = value.AsSpan();
                        var nameSpan = valueSpan.Slice(0, value.Length - 5);
                        var discriminatorSpan = valueSpan.Slice(hashIndex + 1);
                        return x.Name.AsSpan().Equals(nameSpan, _comparison)
                            && x.Discriminator.AsSpan().Equals(discriminatorSpan, default);
                    });
                }

                if (member == null)
                {
                    // TODO: custom result type returning the members?
                    var matchingMembers = members.Where(x => x.Name.Equals(value, _comparison) || x.Nick != null && x.Nick.Equals(value, _comparison)).ToArray();
                    if (matchingMembers.Length > 1)
                        return TypeParserResult<CachedMember>.Unsuccessful("Multiple members found. Mention the member or use their tag or ID.");

                    if (matchingMembers.Length == 1)
                        member = matchingMembers[0];
                }
            }

            return member == null
                ? TypeParserResult<CachedMember>.Unsuccessful("No member found matching the input.")
                : TypeParserResult<CachedMember>.Successful(member);
        }
    }
}