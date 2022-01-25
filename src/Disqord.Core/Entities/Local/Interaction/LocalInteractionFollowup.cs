namespace Disqord
{
    public class LocalInteractionFollowup : LocalMessageBase
    {
        public bool IsEphemeral
        {
            get => Flags.HasFlag(MessageFlag.Ephemeral);
            set
            {
                if (value)
                    Flags |= MessageFlag.Ephemeral;
                else
                    Flags &= ~MessageFlag.Ephemeral;
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
