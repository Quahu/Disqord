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

        public override ValueTask<TypeParserResult<TChannel>> ParseAsync(Parameter parameter, string value, CommandContext _)
        {
            var context = (DiscordCommandContext) _;
            if (context.Guild == null)
                throw new InvalidOperationException("This can only be used in a guild.");

            var channels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, TChannel>(context.Guild._channels);
            TChannel channel = null;
            if (Discord.TryParseChannelMention(value, out var id) || Snowflake.TryParse(value, out id))
                channels.TryGetValue(id, out channel);

            var values = channels.Values;
            if (channel == null)
                channel = values.FirstOrDefault(x => x.Name == value);

            if (channel == null && typeof(CachedTextChannel).IsAssignableFrom(typeof(TChannel)) && value.StartsWith("#"))
                channel = values.FirstOrDefault(x => x.Name.AsSpan().Equals(value.AsSpan().Slice(1), StringComparison.Ordinal));

            return channel == null
                ? new TypeParserResult<TChannel>("No channel found matching the input.")
                : new TypeParserResult<TChannel>(channel);
        }
    }
}