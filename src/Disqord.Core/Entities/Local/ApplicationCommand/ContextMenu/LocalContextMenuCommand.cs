namespace Disqord;

public abstract class LocalContextMenuCommand : LocalApplicationCommand, ILocalConstruct<LocalContextMenuCommand>
{
    public static LocalUserContextMenuCommand User(string name)
    {
        return new LocalUserContextMenuCommand().WithName(name);
    }

    public static LocalMessageContextMenuCommand Message(string name)
    {
        return new LocalMessageContextMenuCommand().WithName(name);
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalContextMenuCommand"/>.
    /// </summary>
    protected LocalContextMenuCommand()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalContextMenuCommand"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalContextMenuCommand(LocalContextMenuCommand other)
        : base(other)
    { }

    /// <inheritdoc />
    public abstract override LocalContextMenuCommand Clone();
}
