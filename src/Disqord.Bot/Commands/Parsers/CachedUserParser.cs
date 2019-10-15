using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedUserParser : TypeParser<CachedUser>
    {
        public static CachedUserParser Instance => _instance ?? (_instance = new CachedUserParser());

        private static CachedUserParser _instance;

        private CachedUserParser()
        { }

        public override ValueTask<TypeParserResult<CachedUser>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            var Context = (DiscordCommandContext) context;
            IReadOnlyDictionary<Snowflake, CachedUser> users;
            if (Context.Guild != null)
            {
                users = new ReadOnlyUpcastingDictionary<Snowflake, CachedMember, CachedUser>(Context.Guild.Members);
            }
            else
            {
                if (Context.Channel is CachedDmChannel dmChannel)
                {
                    users = new Dictionary<Snowflake, CachedUser>
                    {
                        [dmChannel.Recipient.Id] = dmChannel.Recipient,
                        [Context.Bot.CurrentUser.Id] = Context.Bot.CurrentUser
                    };
                }
                else if (Context.Channel is CachedGroupDmChannel groupDmChannel)
                {
                    var dictionary = groupDmChannel.Recipients.ToDictionary(x => x.Key, x => x.Value);
                    dictionary[Context.Bot.CurrentUser.Id] = Context.Bot.CurrentUser;
                    users = dictionary;
                }
                else
                {
                    throw new InvalidOperationException("Unknown channel type.");
                }
            }

            CachedUser user = null;
            if (Discord.TryParseUserMention(value, out var id) || Snowflake.TryParse(value, out id))
                users.TryGetValue(id, out user);

            if (user == null)
            {
                var hashIndex = value.LastIndexOf('#');
                if (hashIndex != -1 && hashIndex + 5 == value.Length)
                    user = users.Values.FirstOrDefault(x => x.Name == value.Substring(0, value.Length - 5) && x.Discriminator == value.Substring(hashIndex + 1));
            }

            if (user == null)
            {
                var matchingUsers = users.Values.Where(x => x.Name == value || x is CachedMember member && member.Nick == value).ToArray();
                if (matchingUsers.Length > 1)
                    return TypeParserResult<CachedUser>.Unsuccessful("Multiple matches found. Mention the user, use their tag or their ID.");

                if (matchingUsers.Length == 1)
                    user = matchingUsers[0];
            }

            return user == null
                ? TypeParserResult<CachedUser>.Unsuccessful("No user found matching the input.")
                : TypeParserResult<CachedUser>.Successful(user);
        }
    }
}