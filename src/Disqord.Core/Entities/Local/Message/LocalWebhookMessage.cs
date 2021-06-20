namespace Disqord
{
    public class LocalWebhookMessage : LocalMessageBase
    {
        public string Name { get; set; }

        public string AvatarUrl { get; set; }

        public LocalWebhookMessage()
        { }

        private LocalWebhookMessage(LocalWebhookMessage other)
            : base(other)
        {
            Name = other.Name;
            AvatarUrl = other.AvatarUrl;
        }

        public override LocalWebhookMessage Clone()
            => new(this);

        public override void Validate()
        {
            base.Validate();
        }
    }
}
