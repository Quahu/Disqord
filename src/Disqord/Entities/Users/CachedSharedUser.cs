using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Qommon.Collections;

namespace Disqord
{
    internal sealed class CachedSharedUser : CachedUser
    {
        public override UserStatus Status => _status;
        private UserStatus _status;

        public override IReadOnlyDictionary<UserClient, UserStatus> Statuses
        {
            get
            {
                if (IsBot)
                    throw new InvalidOperationException("Bots do not support multiple statuses.");

                return _statuses;
            }
        }
        private IReadOnlyDictionary<UserClient, UserStatus> _statuses;

        public override Activity Activity => _activity;
        private Activity _activity;

        public override IReadOnlyList<Activity> Activities
        {
            get
            {
                if (IsBot)
                    throw new InvalidOperationException("Bots do not support multiple activities.");

                return _activities;
            }
        }
        private IReadOnlyList<Activity> _activities;

        internal override CachedSharedUser SharedUser => this;

        internal CachedSharedUser(DiscordClient client, UserModel model) : base(client, model)
        {
            Update(model);
        }

        internal int References
        {
            get => Volatile.Read(ref _references);
            set
            {
                Interlocked.Exchange(ref _references, value);
                if (value == 0)
                    Client._users.TryRemove(Id, out _);
            }
        }
        private int _references;

        internal override void Update(PresenceUpdateModel model)
        {
            _status = model.Status;
            _activity = model.Game != null
                ? Activity.Create(model.Game)
                : null;

            if (!IsBot)
            {
                _statuses = new ReadOnlyDictionary<UserClient, UserStatus>(model.ClientStatus);
                _activities = model.Activities?.Select(x => Activity.Create(x)).ToImmutableArray() ?? ImmutableArray<Activity>.Empty;
            }
        }
    }
}
