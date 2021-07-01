namespace Disqord
{
    public static class LocalEmbedFooterExtensions
    {
        public static LocalEmbedFooter WithText(this LocalEmbedFooter footer, string text)
        {
            footer.Text = text;
            return footer;
        }

        public static LocalEmbedFooter WithIconUrl(this LocalEmbedFooter footer, string iconUrl)
        {
            footer.IconUrl = iconUrl;
            return footer;
        }
    }
}
