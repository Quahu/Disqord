namespace Disqord
{
    public sealed class ModifyMessageProperties
    {
        public Optional<string> Content { internal get; set; }

        public Optional<Embed> Embed { internal get; set; }

        internal ModifyMessageProperties()
        { }

        internal bool HasValues
            => Content.HasValue || Embed.HasValue;
    }
}
