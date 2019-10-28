using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    public sealed class CachedUnknownUser : CachedUser
    {
        public override UserStatus Status => default;

        public override IReadOnlyDictionary<UserClient, UserStatus> Statuses => ImmutableDictionary<UserClient, UserStatus>.Empty;

        public override Activity Activity => default;

        public override IReadOnlyList<Activity> Activities => ImmutableArray<Activity>.Empty;

        internal override CachedSharedUser SharedUser { get; }

        internal CachedUnknownUser(DiscordClient client, UserModel model) : base(client, model)
        {
            Update(model);
        }

        internal override void Update(PresenceUpdateModel model)
            => throw new InvalidOperationException();
    }
}
