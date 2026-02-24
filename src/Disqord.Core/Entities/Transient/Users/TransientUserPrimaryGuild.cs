using Disqord.Models;

namespace Disqord;

public class TransientUserPrimaryGuild : TransientEntity<UserPrimaryGuildJsonModel>, IUserPrimaryGuild
{
    /// <inheritdoc/>
    public Snowflake? GuildId => Model.IdentityGuildId;

    /// <inheritdoc/>
    public bool? IsIdentityEnabled => Model.IdentityEnabled;

    /// <inheritdoc/>
    public string? Tag => Model.Tag;

    /// <inheritdoc/>
    public string? BadgeHash => Model.Badge;

    public TransientUserPrimaryGuild(UserPrimaryGuildJsonModel model)
        : base(model)
    { }
}
