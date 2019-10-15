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

        public override ValueTask<TypeParserResult<CachedMember>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            var Context = (DiscordCommandContext) context;
            if (Context.Guild == null)
                return TypeParserResult<CachedMember>.Unsuccessful("This command must be used a guild.");

            CachedMember member = null;
            if (Discord.TryParseUserMention(value, out var id) || Snowflake.TryParse(value, out id))
                Context.Guild.Members.TryGetValue(id, out member);

            if (member == null)
            {
                var hashIndex = value.LastIndexOf('#');
                if (hashIndex != -1 && hashIndex + 5 == value.Length)
                    member = Context.Guild.Members.Values.FirstOrDefault(x => x.Name == value.Substring(0, value.Length - 5) && x.Discriminator == value.Substring(hashIndex + 1));
            }

            if (member == null)
            {
                var matchingMembers = Context.Guild.Members.Values.Where(x => x.Name == value || x.Nick == value).ToArray();
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