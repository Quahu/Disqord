using System.Threading;
using Disqord.Models;
using PresenceUpdateDispatch = Disqord.Models.Dispatches.PresenceUpdateModel;

namespace Disqord
{
    internal sealed class CachedSharedUser : CachedUser
    {
        public override string Name { get; protected set; }

        public override string Discriminator { get; protected set; }

        public override string AvatarHash { get; protected set; }

        public override bool IsBot { get; }

        public override UserStatus Status => _status;
        private UserStatus _status;

        public override Activity Activity => _activity;
        private Activity _activity;

        internal override CachedSharedUser SharedUser => this;

        internal CachedSharedUser(DiscordClient client, UserModel model) : base(client, model.Id)
        {
            IsBot = model.Bot;
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

        internal override void Update(UserModel model)
        {
            if (model.Username.HasValue)
                Name = model.Username.Value;

            if (model.Discriminator.HasValue)
                Discriminator = model.Discriminator.Value;

            if (model.Avatar.HasValue)
                AvatarHash = model.Avatar.Value;
        }

        internal override void Update(PresenceUpdateDispatch model)
        {
            _status = model.Status;
            _activity = model.Activity != null
                ? Activity.Create(model.Activity)
                : null;

            base.Update(model);
        }
    }
}
