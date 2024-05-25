using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalPollMediaExtensions
{
    public static TPollMedia WithText<TPollMedia>(this TPollMedia media, string text)
        where TPollMedia : LocalPollMedia
    {
        media.Text = text;
        return media;
    }

    public static TPollMedia WithEmoji<TPollMedia>(this TPollMedia media, LocalEmoji emoji)
        where TPollMedia : LocalPollMedia
    {
        media.Emoji = emoji;
        return media;
    }
}
