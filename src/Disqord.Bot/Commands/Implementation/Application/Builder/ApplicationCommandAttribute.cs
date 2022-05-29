using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

public abstract class ApplicationCommandAttribute : CommandAttribute
{
    public string Alias { get; }

    protected ApplicationCommandAttribute(string alias)
    {
        Alias = alias;
    }

    /// <summary>
    ///     Gets the <see cref="ApplicationCommandType"/> of this attribute.
    /// </summary>
    /// <returns></returns>
    protected abstract ApplicationCommandType GetCommandType();

    /// <inheritdoc/>
    public override void Apply(ICommandBuilder builder)
    {
        var slashBuilder = Guard.IsAssignableToType<ApplicationCommandBuilder>(builder);
        slashBuilder.Alias = Alias;
        slashBuilder.Type = GetCommandType();
    }
}
