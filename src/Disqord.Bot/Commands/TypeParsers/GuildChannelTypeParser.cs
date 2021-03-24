using System;
using System.Collections.Generic;
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
        public override ValueTask<TypeParserResult<TChannel>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            if (!context.Bot.CacheProvider.TryGetChannels(context.GuildId, out var cache))
                throw new InvalidOperationException($"The {GetType().Name} requires the channel cache.");

            // Wraps the cache in a pattern-matching wrapper dictionary.
            var channels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, TChannel>(cache);
            TChannel channel;
            if (Mention.TryParseChannel(value, out var id) || Snowflake.TryParse(value, out id))
            {
                // The value is a mention or an id.
                channel = channels.GetValueOrDefault(id);
            }
            else
            {
                // The value is possibly a name.
                channel = channels.Values.FirstOrDefault(x => x.Name == value);
            }

            if (channel != null)
                return Success(channel);

            return Failure("No channel found matching the input.");
        }
    }
}
