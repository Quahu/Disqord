namespace Disqord.Gateway
{
    public class RichActivityAsset
    {
        public string Id { get; }

        public string Text { get; }

        // TODO: change to an extension method to be consistent?
        public string Url => _applicationId != null && Id != null
            ? Discord.Cdn.GetApplicationAssetUrl(_applicationId.Value, Id)
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
