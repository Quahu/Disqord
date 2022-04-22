using System;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientIntegration : TransientClientEntity<IntegrationJsonModel>, IIntegration
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string Type => Model.Type;

        /// <inheritdoc/>
        public bool IsEnabled => Model.Enabled;

        /// <inheritdoc/>
        public bool IsSyncing => Model.Syncing.GetValueOrDefault();

        /// <inheritdoc/>
        public Snowflake? RoleId => Model.RoleId.GetValueOrNullable();

        /// <inheritdoc/>
        public bool EnablesEmojis => Model.EnableEmoticons.GetValueOrDefault();

        /// <inheritdoc/>
        public IntegrationExpirationBehavior? ExpirationBehavior => Model.ExpireBehavior.GetValueOrNullable();

        /// <inheritdoc/>
        public TimeSpan? ExpirationGracePeriod => Optional.ConvertOrDefault(Model.ExpireGracePeriod, x => (TimeSpan?) TimeSpan.FromDays(x));

        /// <inheritdoc/>
        public IUser User => _user ??= Optional.ConvertOrDefault(Model.User, (model, client) => new TransientUser(client, model), Client);
        private TransientUser _user;

        /// <inheritdoc/>
        public IIntegrationAccount Account => _account ??= new TransientIntegrationAccount(Client, Model.Account);
        private TransientIntegrationAccount _account;

        /// <inheritdoc/>
        public DateTimeOffset? SyncedAt => Model.SyncedAt.GetValueOrNullable();

        /// <inheritdoc/>
        public int? SubscriberCount => Model.SubscriberCount.GetValueOrNullable();

        /// <inheritdoc/>
        public bool IsRevoked => Model.Revoked.GetValueOrDefault();

        /// <inheritdoc/>
        public IIntegrationApplication Application => _application ??= Optional.ConvertOrDefault(Model.Application, (model, client) => new TransientIntegrationApplication(client, model), Client);
        private TransientIntegrationApplication _application;

        public TransientIntegration(IClient client, Snowflake guildId, IntegrationJsonModel model)
            : base(client, model)
        {
            GuildId = guildId;
        }
    }
}
