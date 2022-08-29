using Qommon;

namespace Disqord;

public class LocalLinkButtonComponent : LocalButtonComponentBase, ILocalConstruct<LocalLinkButtonComponent>
{
    /// <summary>
    ///     Gets or sets the URL of this link button component.
    /// </summary>
    /// <remarks>
    ///     This property is required.
    /// </remarks>
    public Optional<string> Url { get; set; }

    public LocalLinkButtonComponent()
    { }

    protected LocalLinkButtonComponent(LocalLinkButtonComponent other)
        : base(other)
    {
        Url = other.Url;
    }

    /// <inheritdoc/>
    public override LocalLinkButtonComponent Clone()
    {
        return new(this);
    }

    /// <summary>
    ///     Converts the specified button component to a <see cref="LocalLinkButtonComponent"/>.
    /// </summary>
    /// <remarks>
    ///     The button component must be a link button.
    /// </remarks>
    /// <param name="buttonComponent"> The button component to convert. </param>
    /// <returns>
    ///     The output <see cref="LocalLinkButtonComponent"/>.
    /// </returns>
    public new static LocalLinkButtonComponent CreateFrom(IButtonComponent buttonComponent)
    {
        Guard.IsTrue(buttonComponent.Style == ButtonComponentStyle.Link);

        var localButtonComponent = new LocalLinkButtonComponent
        {
            Url = buttonComponent.Url!
        };

        PopulateFrom(localButtonComponent, buttonComponent);
        return localButtonComponent;
    }
}
