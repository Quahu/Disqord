using System.Collections.Generic;
using System.ComponentModel;
using Qommon.Collections;
using Disqord.Gateway.Api.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway
{
    public class CachedPresence : CachedSnowflakeEntity, IPresence
    {
        /// <inheritdoc/>
        public Snowflake GuildId { get; }

        /// <inheritdoc/>
        public Snowflake MemberId { get; }

        /// <inheritdoc/>
        public IReadOnlyList<IActivity> Activities
        {
            get
            {
                var raw = _activities;
                if (raw is ActivityJsonModel[] models)
                {
                    lock (this)
                    {
                        var activities = models.ToReadOnlyList(Client, (models, client) => TransientActivity.Create(client, models));
                        _activities = activities;
                        return activities;
                    }
                }

                return raw as IReadOnlyList<IActivity>;
            }
        }
        private object _activities;

        /// <inheritdoc/>
        public UserStatus Status { get; private set; }

        /// <inheritdoc/>
        public IReadOnlyDictionary<UserClient, UserStatus> Statuses { get; private set; }

        public CachedPresence(IGatewayClient client, PresenceJsonModel model)
            : base(client, model.User.Id)
        {
            GuildId = model.GuildId;
            MemberId = model.User.Id;

            Update(model);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void Update(PresenceJsonModel model)
        {
            lock (this)
            {
                _activities = model.Activities;
            }

            Status = model.Status;
            Statuses = model.ClientStatus;
        }
    }
}
