using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestWidget : RestDiscordEntity
    {
        public bool IsEnabled { get; private set; }

        public Snowflake? ChannelId { get; private set; }

        public Snowflake GuildId { get; }

        internal RestWidget(RestDiscordClient client, WidgetModel model, Snowflake guildId) : base(client)
        {
            IsEnabled = model.Enabled;
            ChannelId = model.ChannelId;
            GuildId = guildId;
        }

        public async Task ModifyAsync(Action<ModifyWidgetProperties> action, RestRequestOptions options = null)
        {
            var widget = await Client.ModifyWidgetAsync(GuildId, action, options).ConfigureAwait(false);
            IsEnabled = widget.IsEnabled;
            ChannelId = widget.ChannelId;
        }
    }
}
