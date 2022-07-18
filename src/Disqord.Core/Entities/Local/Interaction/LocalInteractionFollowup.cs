using Qommon;

namespace Disqord;

/// <summary>
///     Represents a local interaction followup.
/// </summary>
public class LocalInteractionFollowup : LocalMessageBase, ILocalConstruct<LocalInteractionFollowup>
{
    /// <summary>
    ///     Gets or sets whether this followup is ephemeral,
    ///     i.e. whether it is only visible to the author of the interaction.
    /// </summary>
    /// <remarks>
    ///     This property is ignored if this followup is the first followup message to an interaction deferral response.
    ///     In that case the ephemerality of this followup will be determined by the value in the original deferral response.
    /// </remarks>
    public bool IsEphemeral
    {
        get => Flags.HasValue && Flags.Value.HasFlag(MessageFlags.Ephemeral);
        set
        {
            if (value)
            {
                Flags = Flags.GetValueOrDefault() | MessageFlags.Ephemeral;
            }
            else
            {
                if (!Flags.HasValue)
                    return;

                Flags = Flags.Value & ~MessageFlags.Ephemeral;
            }
        }
    }

    /// <summary>
    ///     Instantiates a new <see cref="LocalInteractionFollowup"/>.
    /// </summary>
    public LocalInteractionFollowup()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalInteractionFollowup"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalInteractionFollowup(LocalInteractionFollowup other)
        : base(other)
    {
        Flags = other.Flags;
    }

    /// <inheritdoc/>
    public override LocalInteractionFollowup Clone()
    {
        return new(this);
    }
}
