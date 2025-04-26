using Qommon;

namespace Disqord;

public class LocalFileComponent : LocalComponent, ILocalSpoilerable, ILocalConstruct<LocalFileComponent>
{
    /// <summary>
    ///     Gets or sets the file of this component.
    /// </summary>
    public new Optional<LocalUnfurledMediaItem> File { get; set; }

    /// <summary>
    ///     Gets or sets whether this file is a spoiler.
    /// </summary>
    public Optional<bool> IsSpoiler { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalFileComponent"/>.
    /// </summary>
    public LocalFileComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalFileComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalFileComponent(LocalFileComponent other)
    {
        File = other.File.Clone();
        IsSpoiler = other.IsSpoiler;
    }

    /// <inheritdoc/>
    public override LocalFileComponent Clone()
    {
        return new(this);
    }
}
