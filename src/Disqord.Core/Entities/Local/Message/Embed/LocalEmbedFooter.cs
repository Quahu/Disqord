namespace Disqord
{
    public sealed class LocalEmbedFooter
    {
        public string Text { get; }

        public string IconUrl { get; }

        internal LocalEmbedFooter(LocalEmbedFooterBuilder builder)
        {
            Text = builder.Text;
            IconUrl = builder.IconUrl;
        }
    }
}
