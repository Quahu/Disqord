namespace Disqord
{
    public sealed class Attachment
    {
        public Snowflake Id { get; internal set; }

        public string FileName { get; internal set; }

        public int Size { get; internal set; }

        public string Url { get; internal set; }

        public string ProxyUrl { get; internal set; }

        public int? Height { get; internal set; }

        public int? Width { get; internal set; }

        internal Attachment()
        { }
    }
}
