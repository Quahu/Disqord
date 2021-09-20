namespace Disqord
{
    public class LocalMessageContextMenuCommand : LocalApplicationCommand
    {
        public LocalMessageContextMenuCommand(string name)
            : base(name)
        { }

        protected LocalMessageContextMenuCommand(LocalApplicationCommand other)
            : base(other)
        { }

        public override LocalMessageContextMenuCommand Clone()
            => new(this);
    }
}
