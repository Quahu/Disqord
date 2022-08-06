using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Ensures the input <see cref="IUser"/> argument is an <see cref="IMember"/>.
/// </summary>
public class MemberParameterCheck : IParameterCheck<IUser>
{
    /// <summary>
    ///     Gets the singleton instance.
    /// </summary>
    public static MemberParameterCheck Instance { get; } = new();

    /// <inheritdoc/>
    public bool ChecksCollection => false;

    protected MemberParameterCheck()
    { }

    /// <inheritdoc/>
    public ValueTask<IResult> CheckAsync(ICommandContext context, IParameter parameter, IUser value)
    {
        if (value is IMember)
            return Results.Success;

        return Results.Failure("The provided user argument must be a member of this guild.");
    }
}
