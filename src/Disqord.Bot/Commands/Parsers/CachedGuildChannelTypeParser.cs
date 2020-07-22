using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public sealed class CachedGuildChannelTypeParser<TChannel> : TypeParser<TChannel> where TChannel : CachedGuildChannel
    {
        private static readonly bool _isText = typeof(CachedTextChannel).IsAssignableFrom(typeof(TChannel));

        private readonly StringComparison _comparison;

        public CachedGuildChannelTypeParser(StringComparison comparison = default)
        {
            _comparison = comparison;
        }

        public override ValueTask<TypeParserResult<TChannel>> ParseAsync(Parameter parameter, string value, CommandContext _)
        {
            var context = (DiscordCommandContext) _;
            if (context.Guild == null)
                throw new InvalidOperationException("This can only be executed in a guild.");

            var channels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, TChannel>(context.Guild._channels);
            TChannel channel = null;
            if (Discord.TryParseChannelMention(value, out var id) || Snowflake.TryParse(value, out id))
                channels.TryGetValue(id, out channel);

            if (channel == null)
            {
                var values = channels.Values;
                channel = values.FirstOrDefault(x => x.Name.Equals(value, _comparison));

                if (channel == null && _isText && value.StartsWith('#'))
                    channel = values.FirstOrDefault(x => x.Name.AsSpan().Equals(value.AsSpan().Slice(1), _comparison));
            }

            return channel == null
                ? new TypeParserResult<TChannel>("No channel found matching the input.")
                : new TypeParserResult<TChannel>(channel);
        }
    }
}