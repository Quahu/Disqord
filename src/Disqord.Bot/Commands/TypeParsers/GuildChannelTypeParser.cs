using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway;
using Qmmands;
using Qommon.Collections;

namespace Disqord.Bot.Commands.Parsers;

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
    where TChannel : class, IGuildChannel
{
    private static readonly string ChannelString;

    /// <inheritdoc/>
    public override ValueTask<ITypeParserResult<TChannel>> ParseAsync(IDiscordGuildCommandContext context, IParameter parameter, ReadOnlyMemory<char> value)
    {
        if (!context.Bot.CacheProvider.TryGetChannels(context.GuildId, out var channelCache))
            throw new InvalidOperationException($"The {GetType().Name} requires the channel cache.");

        // Wraps the cache in a pattern-matching wrapper dictionary.
        var channels = new ReadOnlyOfTypeDictionary<Snowflake, CachedGuildChannel, TChannel>(channelCache);
        TChannel? foundChannel = null;
        var valueSpan = value.Span;
        if (Snowflake.TryParse(valueSpan, out var id) || Mention.TryParseChannel(valueSpan, out id))
        {
            // The value is a mention or an id.
            foundChannel = channels.GetValueOrDefault(id);
        }
        else
        {
            // The value is possibly a name.
            foreach (var channel in channels.Values)
            {
                if (!valueSpan.Equals(channel.Name, StringComparison.Ordinal))
                    continue;

                foundChannel = channel;
                break;
            }
        }

        if (foundChannel != null)
            return Success(foundChannel);

        return Failure($"No {ChannelString} found matching the input.");
    }

    static GuildChannelTypeParser()
    {
        var type = typeof(TChannel);
        ChannelString = type != typeof(IGuildChannel) && type.IsInterface
            ? $"{type.Name[1..type.Name.IndexOf("Channel", StringComparison.Ordinal)].Replace("Guild", "").ToLower()} channel"
            : "channel";
    }
}
