namespace Disqord
{
    public static class LocalEmbedFooterExtensions
    {
        public static TEmbedFooter WithText<TEmbedFooter>(this TEmbedFooter footer, string text)
            where TEmbedFooter : LocalEmbedFooter
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
}
