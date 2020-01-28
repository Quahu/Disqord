using System;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestWebhook : RestSnowflakeEntity
    {
        public Snowflake GuildId { get; }

        public RestFetchable<RestGuild> Guild { get; }

        public WebhookType Type { get; }

        public Snowflake ChannelId { get; private set; }

        public RestFetchable<RestTextChannel> Channel { get; }

        public RestUser Owner { get; private set; }

        public string Name { get; private set; }

        public string AvatarHash { get; private set; }

        public string Token { get; }

        internal RestWebhook(RestDiscordClient client, WebhookModel model) : base(client, model.Id)
        {
            Type = model.Type;
            Token = model.Token;
            GuildId = model.GuildId;
            Guild = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetGuildAsync(@this.GuildId, options));
            Channel = RestFetchable.Create(this, (@this, options) =>
                @this.Client.GetChannelAsync<RestTextChannel>(@this.ChannelId, options));
        }

        internal void Update(WebhookModel model)
        {
            ChannelId = model.ChannelId;

            if (model.User != null)
            {
                if (Owner == null)
                    Owner = new RestUser(Client, model.User);
                else
                    Owner.Update(model.User);
            }

            Name = model.Name;
            AvatarHash = model.Avatar;
        }

        public async Task ModifyAsync(Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            if (!Client.HasAuthorization)
                throw new InvalidOperationException(
                    "To modify a webhook without using its token the client must be authorized. " +
                    "Did you mean to call ModifyWithTokenAsync instead?");

            var model = await Client.InternalModifyWebhookAsync(Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public async Task ModifyWithTokenAsync(Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyWebhookAsync(Id, Token, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task DeleteAsync(RestRequestOptions options = null)
        {
            if (!Client.HasAuthorization)
                throw new InvalidOperationException(
                    "To delete a webhook without using its token the client must be authorized. " +
                    "Did you mean to call DeleteWithTokenAsync instead?");

            return Client.DeleteWebhookAsync(Id, options);
        }

        public Task DeleteWithTokenAsync(RestRequestOptions options = null)
            => Client.DeleteWebhookAsync(Id, Token, options);

        public string GetAvatarUrl(ImageFormat format = default, int size = 2048)
            => Discord.Internal.GetAvatarUrl(this, format, size);
    }
}
