using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientIntegration : TransientEntity<IntegrationJsonModel>, IIntegration
    {
        public Snowflake Id => Model.Id;

        public DateTimeOffset CreatedAt => Id.CreatedAt;

        public Snowflake GuildId { get; }

        public string Name => Model.Name;

        public string Type => Model.Type;

        public bool IsEnabled => Model.Enabled;

        public bool IsSyncing => Model.Syncing.GetValueOrDefault();

        public Snowflake? RoleId => Model.RoleId.GetValueOrNullable();

        public bool EnablesEmojis => Model.EnableEmoticons.GetValueOrDefault();

        public IntegrationExpirationBehavior? ExpirationBehavior => Model.ExpireBehavior.GetValueOrNullable();

        public TimeSpan? ExpirationGracePeriod => Optional.ConvertOrDefault(Model.ExpireGracePeriod, x => (TimeSpan?) TimeSpan.FromDays(x));

        public IUser User => _user ??= Optional.ConvertOrDefault(Model.User, (model, client) => new TransientUser(client, model), Client);
        private TransientUser _user;

        public IIntegrationAccount Account => _account ??= new TransientIntegrationAccount(Client, Model.Account);
        private TransientIntegrationAccount _account;

        public DateTimeOffset? SyncedAt => Model.SyncedAt.GetValueOrNullable();

        public int? SubscriberCount => Model.SubscriberCount.GetValueOrNullable();

        public bool IsRevoked => Model.Revoked.GetValueOrDefault();

        public IIntegrationApplication Application => _application ??= Optional.ConvertOrDefault(Model.Application, (model, client) => new TransientIntegrationApplication(client, model), Client);
        private TransientIntegrationApplication _application;

        public TransientIntegration(IClient client, Snowflake guildId, IntegrationJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
