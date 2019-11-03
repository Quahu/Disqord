using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models.Dispatches;
using Qommon.Collections;

namespace Disqord
{
    public sealed class Presence
    {
        public UserStatus Status { get; }

        public IReadOnlyDictionary<UserClient, UserStatus> Statuses => _statuses ?? throw new InvalidOperationException("Bots cannot have multiple statuses.");
        private IReadOnlyDictionary<UserClient, UserStatus> _statuses;

        public Activity Activity { get; }

        public IReadOnlyList<Activity> Activities => _activities ?? throw new InvalidOperationException("Bots cannot have multiple activities.");
        private IReadOnlyList<Activity> _activities;

        internal Presence(bool isBot, PresenceUpdateModel model)
        {
            Status = model.Status;
            Activity = model.Game != null
                ? Activity.Create(model.Game)
                : null;

            if (!isBot)
            {
                _statuses = new ReadOnlyDictionary<UserClient, UserStatus>(model.ClientStatus);
                _activities = model.Game != null
                    ? model.Activities.Select(x => Activity.Create(x)).ToImmutableArray()
                    : ImmutableArray<Activity>.Empty;
            }
        }
    }
}
