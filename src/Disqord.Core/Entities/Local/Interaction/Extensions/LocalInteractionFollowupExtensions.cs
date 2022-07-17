using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalInteractionFollowupExtensions
{
    public static TFollowup WithIsEphemeral<TFollowup>(this TFollowup followup, bool isEphemeral = true)
        where TFollowup : LocalInteractionFollowup
    {
        followup.IsEphemeral = isEphemeral;
        return followup;
    }
}