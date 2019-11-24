using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestWebhook : RestSnowflakeEntity
    {
        public Snowflake GuildId { get; }

        public RestDownloadable<RestGuild> Guild { get; }

        public WebhookType Type { get; }

        public Snowflake ChannelId { get; private set; }

        public RestDownloadable<RestTextChannel> Channel { get; }

        public RestUser Owner { get; private set; }

        public string Name { get; private set; }

        public string AvatarHash { get; private set; }

        public string Token { get; }

        internal RestWebhook(RestDiscordClient client, WebhookModel model) : base(client, model.Id)
        {
            Type = model.Type;
            Token = model.Token;
            GuildId = model.GuildId;
            Guild = new RestDownloadable<RestGuild>(options => Client.GetGuildAsync(GuildId, options));
            Channel = new RestDownloadable<RestTextChannel>(async options => await client.GetChannelAsync(ChannelId, options).ConfigureAwait(false) as RestTextChannel);
        }

        internal void Update(WebhookModel model)
        {
            ChannelId = model.ChannelId;

            if (model.User != null)
                Owner = new RestUser(Client, model.User);

            Name = model.Name;
            AvatarHash = model.Avatar;
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteWebhookAsync(Id, options);

        public Task DeleteWithTokenAsync(RestRequestOptions options = null)
            => Client.DeleteWebhookWithTokenAsync(Id, Token, options);

        public async Task ModifyAsync(Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyWebhookAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public async Task ModifyWithTokenAsync(Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyWebhookWithTokenAsync(Id, Token, action, options).ConfigureAwait(false);
            Update(model);
        }

        public string GetAvatarUrl(ImageFormat format = default, int size = 2048)
            => Discord.Internal.GetAvatarUrl(this, format, size);
    }
}
