namespace Disqord
{
    public sealed partial class RichActivity
    {
        public sealed class RichParty
        {
            public string Id { get; }

            public int? Size { get; }

            public int? MaxSize { get; }

            internal RichParty(string id, int? size, int? maxSize)
            {
                Id = id;
                Size = size;
                MaxSize = maxSize;
            }
        }
    }
}
