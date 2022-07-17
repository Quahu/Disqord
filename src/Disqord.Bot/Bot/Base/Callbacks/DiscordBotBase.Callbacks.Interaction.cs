using System.Threading.Tasks;
using Disqord.Bot.Commands.Application;
using Disqord.Bot.Commands.Components;
using Disqord.Bot.Commands.Interaction;
using Qommon;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    /// <summary>
    ///     Checks if the received interaction should be processed.
    /// </summary>
    /// <remarks>
    ///     By default returns <see langword="true"/>.
    /// </remarks>
    /// <param name="interaction"> The interaction to check. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the work where the result indicates whether the interaction should be processed.
    /// </returns>
    protected virtual ValueTask<bool> OnInteraction(IUserInteraction interaction)
    {
        return new(true);
    }

    /// <summary>
    ///     Creates an <see cref="IDiscordApplicationCommandContext"/> from the provided parameters.
    /// </summary>
    /// <param name="interaction"> The interaction possibly containing the command. </param>
    /// <returns>
    ///     An <see cref="IDiscordApplicationCommandContext"/> or an <see cref="IDiscordApplicationGuildCommandContext"/> for guild messages.
    /// </returns>
    public virtual IDiscordInteractionCommandContext CreateInteractionCommandContext(IUserInteraction interaction)
    {
        IDiscordInteractionCommandContext context;
        if (interaction is IApplicationCommandInteraction applicationCommandInteraction)
        {
            context = interaction.GuildId != null
                ? new DiscordApplicationGuildCommandContext(this, applicationCommandInteraction)
                : new DiscordApplicationCommandContext(this, applicationCommandInteraction);
        }
        else if (interaction is IComponentInteraction or IModalSubmitInteraction)
        {
            context = interaction.GuildId != null
                ? new DiscordComponentGuildCommandContext(this, interaction)
                : new DiscordComponentCommandContext(this, interaction);
        }
        else
        {
            Throw.InvalidOperationException($"Cannot create a command context for interaction of type '{interaction.Type}'.");
            return null!;
        }

        return context;
    }
}
