namespace Disqord
{
    public sealed class ModifyMessageProperties
    {
        public Optional<string> Content { internal get; set; }

        public Optional<LocalEmbed> Embed { internal get; set; }

        public Optional<MessageFlags> Flags { internal get; set; }

        internal ModifyMessageProperties()
        { }

        internal bool HasValues
            => Content.HasValue || Embed.HasValue || Flags.HasValue;
    }
}
