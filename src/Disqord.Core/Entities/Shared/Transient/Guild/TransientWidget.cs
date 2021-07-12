﻿using Disqord.Models;

namespace Disqord
{
    public class TransientWidget : TransientEntity<GuildWidgetSettingsJsonModel>, IWidget
    {
        /// <inheritdoc/>
        public bool Enabled => Model.Enabled;
        
        /// <inheritdoc/>
        public Snowflake ChannelId => Model.ChannelId;
        
        public TransientWidget(IClient client, GuildWidgetSettingsJsonModel model) 
            : base(client, model)
        { }
    }
}