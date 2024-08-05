using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalChannelTagExtensions
{
    public static TChannelTag WithId<TChannelTag>(this TChannelTag channelTag, Snowflake id)
        where TChannelTag : LocalChannelTag
    {
        channelTag.Id = id;
        return channelTag;
    }

    public static TChannelTag WithName<TChannelTag>(this TChannelTag channelTag, string name)
        where TChannelTag : LocalChannelTag
    {
        channelTag.Name = name;
        return channelTag;
    }

    public static TChannelTag WithIsModerated<TChannelTag>(this TChannelTag channelTag, bool isModerated = true)
        where TChannelTag : LocalChannelTag
    {
        channelTag.IsModerated = isModerated;
        return channelTag;
    }

    public static TChannelTag WithEmoji<TChannelTag>(this TChannelTag channelTag, LocalEmoji emoji)
        where TChannelTag : LocalChannelTag
    {
        channelTag.Emoji = emoji;
        return channelTag;
    }
}
