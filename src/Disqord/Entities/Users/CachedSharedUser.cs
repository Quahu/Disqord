using System.Threading;
using Disqord.Models;

namespace Disqord
{
    internal sealed class CachedSharedUser : CachedUser
    {
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
    }
}
