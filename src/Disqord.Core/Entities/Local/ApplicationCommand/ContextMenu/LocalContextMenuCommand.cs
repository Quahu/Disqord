namespace Disqord
{
    public abstract class LocalContextMenuCommand : LocalApplicationCommand
    {
        public static LocalUserContextMenuCommand User(string name)
            => new LocalUserContextMenuCommand().WithName(name);

        public static LocalMessageContextMenuCommand Message(string name)
            => new LocalMessageContextMenuCommand().WithName(name);

        protected LocalContextMenuCommand()
        { }

        protected LocalContextMenuCommand(LocalContextMenuCommand other)
            : base(other)
        { }
    }
}
