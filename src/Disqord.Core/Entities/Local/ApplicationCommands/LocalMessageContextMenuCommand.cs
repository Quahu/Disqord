namespace Disqord
{
    public class LocalMessageContextMenuCommand : LocalContextMenuCommand
    {
        public LocalMessageContextMenuCommand(string name)
            : base(name)
        { }

        protected LocalMessageContextMenuCommand(LocalMessageContextMenuCommand other)
            : base(other)
        { }

        public override LocalMessageContextMenuCommand Clone()
            => new(this);
    }
}
