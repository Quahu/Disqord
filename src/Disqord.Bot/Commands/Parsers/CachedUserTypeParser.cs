using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedUserTypeParser : TypeParser<CachedUser>
    {
        private CachedMemberTypeParser _memberParser;

        private readonly StringComparison _comparison;

        public CachedUserTypeParser(StringComparison comparison = default)
        {
            _comparison = comparison;
        }

        public override ValueTask<TypeParserResult<CachedUser>> ParseAsync(Parameter parameter, string value, CommandContext _)
        {
            var context = (DiscordCommandContext) _;
            if (context.Guild != null)
            {
                var memberParser = _memberParser ?? (_memberParser = parameter.Service.GetSpecificTypeParser<CachedMember, CachedMemberTypeParser>()
                    ?? new CachedMemberTypeParser(_comparison));
                var memberParserResult = _memberParser.ParseAsync(parameter, value, _).Result;
                return memberParserResult.IsSuccessful
                    ? TypeParserResult<CachedUser>.Successful(memberParserResult.Value)
                    : TypeParserResult<CachedUser>.Unsuccessful(memberParserResult.Reason);
            }

            IReadOnlyDictionary<Snowflake, CachedUser> users;
            if (context.Channel is CachedDmChannel dmChannel)
            {
                users = new Dictionary<Snowflake, CachedUser>
                {
                    [dmChannel.Recipient.Id] = dmChannel.Recipient,
                    [context.Bot.CurrentUser.Id] = context.Bot.CurrentUser
                };
            }
            else if (context.Channel is CachedGroupChannel groupChannel)
            {
                var dictionary = groupChannel.Recipients.ToDictionary(x => x.Key, x => x.Value);
                dictionary[context.Bot.CurrentUser.Id] = context.Bot.CurrentUser;
                users = dictionary;
            }
            else
            {
                throw new InvalidOperationException("Unknown channel type.");
            }

            CachedUser user = null;
            if (Discord.TryParseUserMention(value, out var id) || Snowflake.TryParse(value, out id))
                users.TryGetValue(id, out user);

            if (user == null)
            {
                var values = users.Values;
                var hashIndex = value.LastIndexOf('#');
                if (hashIndex != -1 && hashIndex + 5 == value.Length)
                {
                    user = values.FirstOrDefault(x =>
                    {
                        var valueSpan = value.AsSpan();
                        var nameSpan = valueSpan.Slice(0, value.Length - 5);
                        var discriminatorSpan = valueSpan.Slice(hashIndex + 1);
                        return x.Name.AsSpan().Equals(nameSpan, _comparison)
                            && x.Discriminator.AsSpan().Equals(discriminatorSpan, default);
                    });
                }

                if (user == null)
                {
                    // TODO: custom result type returning the users?
                    var matchingUsers = values.Where(x => x.Name.Equals(value, _comparison) || x is CachedMember member && member.Nick != null && member.Nick.Equals(value, _comparison)).ToArray();
                    if (matchingUsers.Length > 1)
                        return TypeParserResult<CachedUser>.Unsuccessful("Multiple users found. Mention the user or use their tag or ID.");

                    if (matchingUsers.Length == 1)
                        user = matchingUsers[0];
                }
            }

            return user == null
                ? TypeParserResult<CachedUser>.Unsuccessful("No user found matching the input.")
                : TypeParserResult<CachedUser>.Successful(user);
        }
    }
}