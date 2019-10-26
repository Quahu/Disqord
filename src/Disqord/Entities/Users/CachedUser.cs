using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Disqord.Models;
using Disqord.Models.Dispatches;
using Qommon.Collections;

namespace Disqord
{
    public abstract partial class CachedUser : CachedSnowflakeEntity, IUser
    {
        public virtual string Name { get; protected set; }

        public virtual string Discriminator { get; protected set; }

        public virtual string AvatarHash { get; protected set; }

        public virtual bool IsBot { get; }

        public string Tag => $"{Name}#{Discriminator}";

        public virtual string Mention => Discord.MentionUser(this);

        public virtual CachedRelationship Relationship
        {
            get
            {
                var currentUser = Client.CurrentUser;
                return currentUser != null && currentUser.Relationships.TryGetValue(Id, out var relationship)
                    ? relationship
                    : null;
            }
        }

        public string Note
        {
            get
            {
                var currentUser = Client.CurrentUser;
                return currentUser != null && currentUser.Notes.TryGetValue(Id, out var note)
                    ? note
                    : null;
            }
        }

        public virtual CachedDmChannel DmChannel => Client.DmChannels.Values.FirstOrDefault(x => x.Recipient.Id == Id);

        public virtual UserStatus Status { get; private set; }

        public IReadOnlyDictionary<UserClient, UserStatus> Statuses
        {
            get
            {
                if (IsBot)
                    throw new InvalidOperationException("Bots do not support multiple statuses.");

                return _statuses;
            }

            private set => _statuses = value;
        }
        private IReadOnlyDictionary<UserClient, UserStatus> _statuses;

        public virtual Activity Activity { get; private set; }

        public virtual IReadOnlyList<Activity> Activities
        {
            get
            {
                if (IsBot)
                    throw new InvalidOperationException("Bots do not support multiple activities.");

                return _activities;
            }

            private set => _activities = value;
        }
        private IReadOnlyList<Activity> _activities;

        internal abstract CachedSharedUser SharedUser { get; }

        internal CachedUser(CachedSharedUser sharedUser) : base(sharedUser.Client, sharedUser.Id)
        {
            IsBot = sharedUser.IsBot;
        }

        internal CachedUser(DiscordClient client, UserModel model) : base(client, model.Id)
        {
            IsBot = model.Bot;
        }

        internal virtual void Update(UserModel model)
        {
            if (model.Username.HasValue)
                Name = model.Username.Value;

            if (model.Discriminator.HasValue)
                Discriminator = model.Discriminator.Value;

            if (model.Avatar.HasValue)
                AvatarHash = model.Avatar.Value;
        }

        internal virtual void Update(PresenceUpdateModel model)
        {
            Status = model.Status;
            Activity = model.Game != null
                ? Activity.Create(model.Game)
                : null;

            if (IsBot)
                return;

            Statuses = new ReadOnlyDictionary<UserClient, UserStatus>(model.ClientStatus);
            Activities = model.Activities?.Select(x => Activity.Create(x)).ToImmutableArray() ?? ImmutableArray<Activity>.Empty;
        }

        internal CachedUser Clone()
            => (CachedUser) MemberwiseClone();

        public string GetAvatarUrl(ImageFormat format = default, int size = 2048)
            => Discord.Internal.GetAvatarUrl(this, format, size);

        public override string ToString()
            => Tag;
    }
}
