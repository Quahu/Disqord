using System;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Gateway;
using Disqord.Utilities;
using Qmmands;

namespace Disqord.Bot.Parsers
{
    public class GuildChannelTypeParser<TChannel> : DiscordGuildTypeParser<TChannel> 
        where TChannel : IGuildChannel
    {
        private static readonly bool _isText = typeof(ITextChannel).IsAssignableFrom(typeof(TChannel));

        private readonly StringComparison _comparison;

        public GuildChannelTypeParser(StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            _comparison = comparison;
        }

        public override ValueTask<TypeParserResult<TChannel>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            if (!context.Bot.CacheProvider.TryGetChannels(context.GuildId, out var cache))
                throw new InvalidOperationException($"The {GetType().Name} requires a channel cache.");
            
            var channels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, TChannel>(cache);
            TChannel channel = default;
            if (Mention.TryParseChannel(value, out var id) || Snowflake.TryParse(value, out id))
                channels.TryGetValue(id, out channel);

            if (channel == null)
                channel = channels.Values.FirstOrDefault(x => x.Name.Equals(value, _comparison));

            if (channel != null)
                return Success(channel);

            return Failure("No channel found matching the input.");
        }
    }
}
