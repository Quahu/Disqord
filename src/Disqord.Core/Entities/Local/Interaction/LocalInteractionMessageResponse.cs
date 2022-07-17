namespace Disqord;

/// <summary>
///     Represents a local interaction message response.
/// </summary>
public class LocalInteractionMessageResponse : LocalInteractionFollowup, ILocalInteractionResponse, ILocalConstruct<LocalInteractionMessageResponse>
{
    public InteractionResponseType Type { get; set; }

    public LocalInteractionMessageResponse()
    { }

    public LocalInteractionMessageResponse(InteractionResponseType type)
    {
        Type = type;
    }

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
