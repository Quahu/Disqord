using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalEmbedFooterExtensions
{
    public static TFooter WithText<TFooter>(this TFooter footer, string text)
        where TFooter : LocalEmbedFooter
    {
        footer.Text = text;
        return footer;
    }

    public static TEmbedFooter WithIconUrl<TEmbedFooter>(this TEmbedFooter footer, string iconUrl)
        where TEmbedFooter : LocalEmbedFooter
    {
        footer.IconUrl = iconUrl;
        return footer;
    }
}
