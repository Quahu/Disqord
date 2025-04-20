using Qommon;

namespace Disqord;

public class LocalTextDisplayComponent : LocalComponent, ILocalConstruct<LocalTextDisplayComponent>
{
    /// <summary>
    ///     Gets or sets the content of this text display.
    /// </summary>
    public Optional<string> Content { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalTextDisplayComponent"/>.
    /// </summary>
    public LocalTextDisplayComponent()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalTextDisplayComponent"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalTextDisplayComponent(LocalTextDisplayComponent other)
    {
        Content = other.Content;
    }

    /// <inheritdoc/>
    public override LocalTextDisplayComponent Clone()
    {
        return new(this);
    }

    public static LocalTextDisplayComponent CreateFrom(ITextDisplayComponent textDisplayComponent)
    {
        return new LocalTextDisplayComponent
        {
            Id = textDisplayComponent.Id,
            Content = textDisplayComponent.Content
        };
    }
}
