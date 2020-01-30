using System.Threading;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed class CachedSharedUser : CachedUser
    {
        public override Presence Presence => _presence;
        private Presence _presence;

        internal override CachedSharedUser SharedUser => this;

        internal CachedSharedUser(DiscordClientBase client, UserModel model) : base(client, model)
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
                    Client.State._users.TryRemove(Id, out _);
            }
        }
        private int _references;

        internal override void Update(PresenceUpdateModel model)
        {
            _presence = model.Status != UserStatus.Offline
                ? new Presence(IsBot, model)
                : null;
        }
    }
}
