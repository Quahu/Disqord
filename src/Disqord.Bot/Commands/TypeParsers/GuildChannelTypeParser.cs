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
    /// <summary>
    ///     Represents type parsing for the <see cref="IGuildChannel"/> type and any derived types.
    ///     Does <b>not</b> support parsing channels that are not in the cache.
    /// </summary>
    /// <remarks>
    ///     Supports the following inputs, in order:
    ///     <list type="number">
    ///         <item>
    ///             <term> ID </term>
    ///             <description> The ID of the channel. </description>
    ///         </item>
    ///         <item>
    ///             <term> Mention </term>
    ///             <description> The mention of the channel. </description>
    ///         </item>
    ///         <item>
    ///             <term> Name </term>
    ///             <description> The name of the channel. This is case-sensitive. </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public class GuildChannelTypeParser<TChannel> : DiscordGuildTypeParser<TChannel>
        where TChannel : IGuildChannel
    {
        private static readonly string ChannelString;

        /// <inheritdoc/>
        public override ValueTask<TypeParserResult<TChannel>> ParseAsync(Parameter parameter, string value, DiscordGuildCommandContext context)
        {
            if (!context.Bot.CacheProvider.TryGetChannels(context.GuildId, out var cache))
                throw new InvalidOperationException($"The {GetType().Name} requires the channel cache.");

            // Wraps the cache in a pattern-matching wrapper dictionary.
            var channels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, TChannel>(cache);
            TChannel channel;
            if (Snowflake.TryParse(value, out var id) || Mention.TryParseChannel(value, out id))
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

            return Failure($"No {ChannelString} found matching the input.");
        }

        static GuildChannelTypeParser()
        {
            var type = typeof(TChannel);
            ChannelString = type != typeof(IGuildChannel) && type.IsInterface
                ? $"{type.Name[1..type.Name.IndexOf("Channel")].ToLower()} channel"
                : "channel";
        }
    }
}
