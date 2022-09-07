using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

/// <summary>
///     Specifies that the <see cref="IUser"/> parameter must not be the author.
/// </summary>
public class RequireNotAuthorAttribute : DiscordParameterCheckAttribute
{
    /// <summary>
    ///     Instantiates a new <see cref="RequireNotAuthorAttribute"/>.
    /// </summary>
    public RequireNotAuthorAttribute()
    { }

    /// <inheritdoc/>
    public override bool CanCheck(IParameter parameter, object? value)
    {
        return value is IUser;
    }

    /// <inheritdoc/>
    public override ValueTask<IResult> CheckAsync(IDiscordCommandContext context, IParameter parameter, object? argument)
    {
        var user = (argument as IUser)!;
        if (user.Id != context.Author.Id)
            return Results.Success;

        return Results.Failure("The provided user argument must be another user.");
    }
}
