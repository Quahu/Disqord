namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Marks the decorated method as a user context menu command.
/// </summary>
/// <remarks>
///     The method must have a single <see cref="IUser"/> parameter.
///     Can be <see cref="IMember"/> for guild commands.
/// </remarks>
public class UserCommandAttribute : ApplicationCommandAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="UserCommandAttribute"/> with the specified alias.
    /// </summary>
    /// <param name="alias"> The alias of the command. </param>
    public UserCommandAttribute(string alias)
        : base(alias)
    { }

    /// <inheritdoc/>
    protected override ApplicationCommandType GetCommandType()
    {
        return ApplicationCommandType.User;
    }
}
