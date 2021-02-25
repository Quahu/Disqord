using Disqord.Models;

namespace Disqord
{
    public class EmbedFooter
    {
        public string Text { get; }

        public string? IconUrl { get; }

        public string? ProxyIconUrl { get; }

        public EmbedFooter(EmbedFooterJsonModel model)
        {
            Text = model.Text;
            IconUrl = model.IconUrl.GetValueOrDefault();
            ProxyIconUrl = model.ProxyIconUrl.GetValueOrDefault();
        }
    }
}
