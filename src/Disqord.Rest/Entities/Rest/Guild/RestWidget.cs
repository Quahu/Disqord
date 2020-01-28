using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestWidget : RestDiscordEntity
    {
        public Snowflake GuildId { get; }

        public RestFetchable<RestGuild> Guild { get; }

        public bool IsEnabled { get; private set; }

        public Snowflake? ChannelId { get; private set; }

        internal RestWidget(RestDiscordClient client, WidgetModel model, Snowflake guildId) : base(client)
        {
            GuildId = guildId;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));
            Update(model);
        }

        internal void Update(WidgetModel model)
        {
            IsEnabled = model.Enabled;
            ChannelId = model.ChannelId;
        }

        public async Task ModifyAsync(Action<ModifyWidgetProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyWidgetAsync(GuildId, action, options).ConfigureAwait(false);
            Update(model);
        }
    }
}
