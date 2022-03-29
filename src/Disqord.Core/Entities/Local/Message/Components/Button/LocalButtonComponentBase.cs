namespace Disqord
{
    public abstract class LocalButtonComponentBase : LocalNestedComponent
    {
        public Optional<string> Label { get; set; }

        public LocalEmoji Emoji { get; set; }

        protected LocalButtonComponentBase()
        { }

        protected LocalButtonComponentBase(LocalButtonComponentBase other)
        {
            Label = other.Label;
            Emoji = other.Emoji;
        }
    }
}
