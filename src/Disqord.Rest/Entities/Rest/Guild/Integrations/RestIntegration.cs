using System;
using Disqord.Models;

namespace Disqord.Rest
{
    // TODO: there's almost no documentation on this
    // https://discordapp.com/developers/docs/resources/guild#integration-object
    public sealed class RestIntegration : RestSnowflakeEntity
    {
        public string Name { get; }

        public string Type { get; }

        public bool IsEnabled { get; }

        public bool IsSyncing { get; }

        public Snowflake RoleId { get; }

        public int ExpireBehavior { get; private set; }

        public TimeSpan ExpireGracePeriod { get; private set; }

        public RestUser User { get; }

        public IntegrationAccount Account { get; }

        public DateTimeOffset SyncedAt { get; }

        internal RestIntegration(RestDiscordClient client, IntegrationModel model) : base(client, model.Id)
        {
            Name = model.Name;
            Type = model.Type;
            IsEnabled = model.Enabled;
            IsSyncing = model.Syncing;
            RoleId = model.RoleId;
            User = new RestUser(client, model.User);
            Account = new IntegrationAccount(model.Account);
            SyncedAt = model.SyncedAt;

            Update(model);
        }

        internal void Update(IntegrationModel model)
        {
            ExpireBehavior = model.ExpireBehavior;
            ExpireGracePeriod = TimeSpan.FromSeconds(model.ExpireGracePeriod);
        }

        public sealed class IntegrationAccount
        {
            public string Id { get; }

            public string Name { get; }

            internal IntegrationAccount(IntegrationAccountModel model)
            {
                Id = model.Id;
                Name = model.Name;
            }
        }
    }
}
