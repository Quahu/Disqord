using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientWebhook : TransientEntity<WebhookJsonModel>, IWebhook
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public Snowflake ChannelId => Model.ChannelId;

        public Snowflake GuildId => Model.GuildId.Value;

        public string Name => Model.Name;

        public string AvatarHash => Model.Avatar;

        public IUser Creator
        {
            get
            {
                if (Model.User.HasValue && _creator == null)
                    _creator = new TransientUser(Client, Model.User.Value);

                return _creator;
            }
        }
        private IUser _creator;

        public string Token => Model.Token.GetValueOrDefault();

        public WebhookType Type => Model.Type;

        public TransientWebhook(IClient client, WebhookJsonModel model)
            : base(client, model)
        { }
    }
}
