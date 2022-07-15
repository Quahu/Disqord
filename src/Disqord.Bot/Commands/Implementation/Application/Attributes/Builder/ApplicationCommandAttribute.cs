using Qmmands;
using Qommon;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Marks the decorated method as an application command.
/// </summary>
public abstract class ApplicationCommandAttribute : CommandAttribute
{
    /// <summary>
    ///     Gets the alias of the command.
    /// </summary>
    public string Alias { get; }

    /// <summary>
    ///     Instantiates a new <see cref="ApplicationCommandAttribute"/> with the specified alias.
    /// </summary>
    /// <param name="alias"> The alias of the command. </param>
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
        var applicationBuilder = Guard.IsAssignableToType<ApplicationCommandBuilder>(builder);
        applicationBuilder.Alias = Alias;
        applicationBuilder.Type = GetCommandType();
    }
}
