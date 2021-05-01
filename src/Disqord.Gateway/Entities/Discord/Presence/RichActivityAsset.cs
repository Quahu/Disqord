namespace Disqord.Gateway
{
    public class RichActivityAsset
    {
        public string Id { get; }

        public string Text { get; }

        // TODO: move to CDN urls?
        public string Url => _applicationId != null
            ? $"{Discord.Cdn.URL}app-assets/{_applicationId}/{Id}.png"
            : null;

        private readonly Snowflake? _applicationId;

        public RichActivityAsset(
            Snowflake? applicationId,
            Optional<string> id,
            Optional<string> text)
        {
            _applicationId = applicationId;
            Id = id.GetValueOrDefault();
            Text = text.GetValueOrDefault();
        }
    }
}
