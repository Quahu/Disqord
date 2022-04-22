using Qommon;

namespace Disqord
{
    public abstract class LocalButtonComponentBase : LocalComponent
    {
        public Optional<string> Label { get; set; }

        public Optional<LocalEmoji> Emoji { get; set; }

        /// <summary>
        ///     Gets or sets whether this button is disabled.
        /// </summary>
        public Optional<bool> IsDisabled { get; set; }

        protected LocalButtonComponentBase()
        { }

        protected LocalButtonComponentBase(LocalButtonComponentBase other)
        {
            Label = other.Label;
            Emoji = other.Emoji;
        }
    }
}
