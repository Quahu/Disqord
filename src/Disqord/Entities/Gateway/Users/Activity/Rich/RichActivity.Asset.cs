namespace Disqord
{
    public sealed partial class RichActivity
    {
        public sealed class RichAsset
        {
            public string Id { get; }

            public string Text { get; }

            // TODO: move to CDN urls?
            public string Url => _applicationId != null
                ? $"https://cdn.discordapp.com/app-assets/{_applicationId}/{Id}.png"
                : null;

            private readonly ulong? _applicationId;

            internal RichAsset(ulong? applicationId, string id, string text)
            {
                _applicationId = applicationId;
                Id = id;
                Text = text;
            }
        }
    }
}
