using Disqord.Models;

namespace Disqord;

public class TransientGuildWidget : TransientClientEntity<GuildWidgetJsonModel>, IGuildWidget
{
    /// <inheritdoc/>
    public Snowflake GuildId { get; }

    /// <inheritdoc/>
    public Snowflake? ChannelId => Model.ChannelId;

    /// <inheritdoc/>
    public bool IsEnabled => Model.Enabled;

    public TransientGuildWidget(IClient client, Snowflake guildId, GuildWidgetJsonModel model)
        : base(client, model)
    {
        GuildId = guildId;
    }
}