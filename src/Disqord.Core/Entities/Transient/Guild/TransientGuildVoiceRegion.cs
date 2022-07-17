using Disqord.Models;

namespace Disqord;

public class TransientGuildVoiceRegion : TransientVoiceRegion, IGuildVoiceRegion
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public bool IsVip => Model.Vip;

    /// <inheritdoc/>
    public bool IsCustom => Model.Custom;

    public TransientGuildVoiceRegion(IClient client, Snowflake guildId, VoiceRegionJsonModel model)
        : base(client, model)
    {
        GuildId = guildId;
    }
}