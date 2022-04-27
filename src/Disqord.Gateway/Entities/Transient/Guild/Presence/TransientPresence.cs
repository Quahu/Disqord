using System.Collections.Generic;
using Qommon.Collections;
using Disqord.Gateway.Api.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway
{
    public class TransientPresence : TransientClientEntity<PresenceJsonModel>, IPresence
    {
        /// <inheritdoc/>
        public Snowflake MemberId => Model.User.Id;

        /// <inheritdoc/>
        public Snowflake GuildId => Model.GuildId;

        /// <inheritdoc/>
        public IReadOnlyList<IActivity> Activities => _activities ??= Model.Activities.ToReadOnlyList(Client, (model, client) => TransientActivity.Create(client, model));
        private IReadOnlyList<IActivity> _activities;

        /// <inheritdoc/>
        public UserStatus Status => Model.Status;

        /// <inheritdoc/>
        public IReadOnlyDictionary<UserClient, UserStatus> Statuses => Model.ClientStatus;

        public TransientPresence(IClient client, PresenceJsonModel model)
            : base(client, model)
        { }
    }
}
