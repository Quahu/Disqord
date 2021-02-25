using Disqord.Models;

namespace Disqord
{
    public sealed class EmbedProvider
    {
        public string? Name { get; }

        public string? Url { get; }

        public EmbedProvider(EmbedProviderJsonModel model)
        {
            Name = model.Name.GetValueOrDefault();
            Url = model.Url.GetValueOrDefault();
        }
    }
}
