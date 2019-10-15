namespace Disqord
{
    public sealed class EmbedFooter
    {
        public string Text { get; internal set; }

        public string IconUrl { get; internal set; }

        public string ProxyIconUrl { get; internal set; }

        internal EmbedFooter()
        { }

        internal EmbedFooter(EmbedFooterBuilder builder)
        {
            Text = builder.Text;
            IconUrl = builder.IconUrl;
        }
    }
}
