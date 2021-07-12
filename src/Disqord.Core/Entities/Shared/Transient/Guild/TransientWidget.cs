using Disqord.Models;

namespace Disqord
{
    public class TransientWidget : TransientEntity<GuildWidgetSettingsJsonModel>, IWidget
    {
        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId;
        
        /// <inheritdoc/>
        public bool IsEnabled => Model.Enabled;
        
        public TransientWidget(IClient client, GuildWidgetSettingsJsonModel model)
            : base(client, model)
        { }
    }
}