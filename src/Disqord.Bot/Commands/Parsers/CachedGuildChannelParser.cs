using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedGuildChannelParser<TChannel> : TypeParser<TChannel> where TChannel : CachedGuildChannel
    {
        public static CachedGuildChannelParser<TChannel> Instance => _instance ?? (_instance = new CachedGuildChannelParser<TChannel>());

        private static CachedGuildChannelParser<TChannel> _instance;

        private CachedGuildChannelParser()
        { }

        public override ValueTask<TypeParserResult<TChannel>> ParseAsync(Parameter parameter, string value, CommandContext context)
        {
            var Context = (DiscordCommandContext) context;
            if (Context.Guild == null)
                return new TypeParserResult<TChannel>("This command must be used a guild.");

            var channels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, TChannel>(Context.Guild.Channels);
            TChannel channel = null;
            if (Discord.TryParseChannelMention(value, out var id) || Snowflake.TryParse(value, out id))
                channels.TryGetValue(id, out channel);

            if (channel == null)
                channel = channels.Values.FirstOrDefault(x => x.Name == value);

            if (channel == null && typeof(CachedTextChannel).IsAssignableFrom(typeof(TChannel)) && value.StartsWith("#"))
                channel = channels.Values.FirstOrDefault(x => x.Name == value.Substring(1));

            return channel == null
                ? new TypeParserResult<TChannel>("No channel found matching the input.")
                : new TypeParserResult<TChannel>(channel);
        }
    }
}