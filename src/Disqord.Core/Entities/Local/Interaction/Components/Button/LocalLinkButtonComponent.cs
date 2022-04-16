namespace Disqord
{
    public class LocalLinkButtonComponent : LocalButtonComponentBase
    {
        /// <summary>
        ///     Gets or sets the URL of this link button component.
        /// </summary>
        public string Url { get; set; }

        public LocalLinkButtonComponent()
        { }

        protected LocalLinkButtonComponent(LocalLinkButtonComponent other)
            : base(other)
        {
            Url = other.Url;
        }

        public override LocalLinkButtonComponent Clone()
            => new(this);
    }
}
