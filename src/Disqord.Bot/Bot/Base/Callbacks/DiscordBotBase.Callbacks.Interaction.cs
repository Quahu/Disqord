using System.Threading.Tasks;
using Disqord.Bot.Commands.Application;

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
    protected virtual ValueTask<bool> OnInteraction(IInteraction interaction)
        => new(true);

    /// <summary>
    ///     Creates an <see cref="IDiscordApplicationCommandContext"/> from the provided parameters.
    /// </summary>
    /// <param name="interaction"> The interaction possibly containing the command. </param>
    /// <returns>
    ///     An <see cref="IDiscordApplicationCommandContext"/> or an <see cref="IDiscordApplicationGuildCommandContext"/> for guild messages.
    /// </returns>
    public virtual IDiscordApplicationCommandContext CreateApplicationCommandContext(IApplicationCommandInteraction interaction)
    {
        var context = interaction.GuildId != null
            ? new DiscordApplicationGuildCommandContext(this, interaction)
            : new DiscordApplicationCommandContext(this, interaction);

        SetAccessorContext(context);
        return context;
    }
}
