using Disqord.Models;

namespace Disqord;

public class TransientVanityInvite : TransientClientEntity<InviteJsonModel>, IVanityInvite
{
    /// <inheritdoc/>
    public string Code => Model.Code;

    /// <inheritdoc/>
    public int Uses => Model.Uses.Value;

    /// <inheritdoc />
    public Snowflake GuildId { get; }

    public TransientVanityInvite(IClient client, Snowflake guildId, InviteJsonModel model)
        : base(client, model)
    {
        GuildId = guildId;
    }
}