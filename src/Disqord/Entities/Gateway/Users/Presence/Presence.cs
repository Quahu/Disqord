using System;
using System.Collections.Generic;
using System.Linq;
using Disqord.Collections;
using Disqord.Models.Dispatches;

namespace Disqord
{
    public sealed class Presence
    {
        public UserStatus Status { get; }

        public IReadOnlyDictionary<UserClient, UserStatus> Statuses => _statuses ?? throw new InvalidOperationException("Bots cannot have multiple statuses.");
        private readonly IReadOnlyDictionary<UserClient, UserStatus> _statuses;

        public Activity Activity { get; }

        public IReadOnlyList<Activity> Activities => _activities ?? throw new InvalidOperationException("Bots cannot have multiple activities.");
        private readonly IReadOnlyList<Activity> _activities;

        internal Presence(bool isBot, PresenceUpdateModel model)
        {
            Status = model.Status;
            Activity = model.Game != null
                ? Activity.Create(model.Game)
                : null;

            if (!isBot)
            {
                _statuses = model.ClientStatus.ReadOnly();
                _activities = model.Game != null
                    ? model.Activities.ToReadOnlyList(x => Activity.Create(x))
                    : ReadOnlyList<Activity>.Empty;
            }
        }
    }
}
