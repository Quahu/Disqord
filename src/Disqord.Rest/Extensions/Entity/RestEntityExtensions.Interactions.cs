using System.Runtime.CompilerServices;
using Qommon;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static readonly ConditionalWeakTable<IInteraction, InteractionResponseHelper> InitialInteractionResponseHelpers = new();
    public static readonly ConditionalWeakTable<IInteraction, InteractionFollowupHelper> FollowupInteractionResponseHelpers = new();

    public static InteractionResponseHelper Response(this IInteraction interaction)
    {
        return InitialInteractionResponseHelpers.GetValue(interaction, interaction => new InteractionResponseHelper(interaction));
    }

    public static InteractionFollowupHelper Followup(this IInteraction interaction)
    {
        if (!InitialInteractionResponseHelpers.TryGetValue(interaction, out var helper) || !helper.HasResponded)
            Throw.InvalidOperationException("You must first respond to the interaction in order to use followups.");

        return FollowupInteractionResponseHelpers.GetValue(interaction, interaction => new InteractionFollowupHelper(interaction));
    }
}