using Disqord.Bot.Commands.Interaction;
using Qommon;

namespace Disqord.Bot.Commands;

public static class DiscordInteractionCommandResultExtensions
{
    /// <summary>
    ///     Marks the response of the <see cref="DiscordInteractionResponseCommandResult"/> as ephemeral,
    ///     i.e. makes it only visible to the author of the interaction.
    /// </summary>
    /// <param name="result"> The result to mark. </param>
    /// <param name="isEphemeral"> Whether the response should be ephemeral. </param>
    /// <returns>
    ///     The same <see cref="DiscordInteractionResponseCommandResult"/>.
    /// </returns>
    public static DiscordInteractionResponseCommandResult AsEphemeral(this DiscordInteractionResponseCommandResult result, bool isEphemeral = true)
    {
        Guard.IsNotNull(result);

        var response = Guard.IsAssignableToType<LocalInteractionMessageResponse>(result.Message);
        response.IsEphemeral = isEphemeral;
        return result;
    }

    /// <summary>
    ///     Marks the deferral of the <see cref="DiscordInteractionDeferralCommandResult"/> as ephemeral,
    ///     i.e. makes the followups only visible to the author of the interaction.
    /// </summary>
    /// <param name="result"> The result to mark. </param>
    /// <param name="isEphemeral"> Whether the followups should be ephemeral. </param>
    /// <returns>
    ///     The same <see cref="DiscordInteractionDeferralCommandResult"/>.
    /// </returns>
    public static DiscordInteractionDeferralCommandResult AsEphemeral(this DiscordInteractionDeferralCommandResult result, bool isEphemeral = true)
    {
        Guard.IsNotNull(result);

        result.IsEphemeral = isEphemeral;
        return result;
    }
}
