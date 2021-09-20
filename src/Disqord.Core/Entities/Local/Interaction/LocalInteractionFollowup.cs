namespace Disqord
{
    public class LocalInteractionFollowup : LocalMessageBase
    {
        public InteractionResponseFlag Flags { get; set; }

        public bool IsEphemeral
        {
            get => Flags.HasFlag(InteractionResponseFlag.Ephemeral);
            set
            {
                if (value)
                    Flags |= InteractionResponseFlag.Ephemeral;
                else
                    Flags &= ~InteractionResponseFlag.Ephemeral;
            }
        }

        public LocalInteractionFollowup()
        { }

        private LocalInteractionFollowup(LocalInteractionFollowup other)
            : base(other)
        {
            Flags = other.Flags;
        }

        public LocalInteractionFollowup WithIsEphemeral(bool isEphemeral = true)
        {
            IsEphemeral = isEphemeral;
            return this;
        }

        public override LocalInteractionFollowup Clone()
            => new(this);
    }
}
