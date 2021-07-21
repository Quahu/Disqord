using Disqord.Models;

namespace Disqord
{
    public class TransientWidget : TransientEntity<GuildWidgetSettingsJsonModel>, IWidget
    {
        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId;

        /// <inheritdoc/>
        public bool IsEnabled => Model.Enabled;

        public TransientWidget(IClient client, Snowflake guildId, GuildWidgetSettingsJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
