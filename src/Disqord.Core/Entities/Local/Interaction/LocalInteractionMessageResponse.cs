namespace Disqord;

/// <summary>
///     Represents a local interaction message response.
/// </summary>
public class LocalInteractionMessageResponse : LocalInteractionFollowup, ILocalInteractionResponse, ILocalConstruct<LocalInteractionMessageResponse>
{
    public InteractionResponseType Type { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalInteractionMessageResponse"/>.
    /// </summary>
    public LocalInteractionMessageResponse()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalInteractionMessageResponse"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalInteractionMessageResponse(LocalInteractionMessageResponse other)
        : base(other)
    {
        Type = other.Type;
        Flags = other.Flags;
    }

    /// <inheritdoc/>
    public override LocalInteractionMessageResponse Clone()
    {
        return new(this);
    }
}
