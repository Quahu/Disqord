using Disqord.Models;

namespace Disqord
{
    public class EmbedAuthor
    {
        public string? Name { get; }

        public string? Url { get; }

        public string? IconUrl { get; }

        public string? ProxyIconUrl { get; }

        public EmbedAuthor(EmbedAuthorJsonModel model)
        {
            Name = model.Name.GetValueOrDefault();
            Url = model.Url.GetValueOrDefault();
            IconUrl = model.IconUrl.GetValueOrDefault();
            ProxyIconUrl = model.ProxyIconUrl.GetValueOrDefault();
        }
    }
}
