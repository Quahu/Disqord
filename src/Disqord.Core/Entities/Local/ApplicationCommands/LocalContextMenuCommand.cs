namespace Disqord
{
    public abstract class LocalContextMenuCommand : LocalApplicationCommand
    {
        protected LocalContextMenuCommand(string name)
            : base(name)
        { }

        protected LocalContextMenuCommand(LocalContextMenuCommand other)
            : base(other)
        { }
    }
}
