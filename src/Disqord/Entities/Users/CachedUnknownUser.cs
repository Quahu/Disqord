using Disqord.Models;

namespace Disqord
{
    public sealed class CachedUnknownUser : CachedUser
    {
        internal override CachedSharedUser SharedUser { get; }

        internal CachedUnknownUser(DiscordClient client, UserModel model) : base(client, model.Id)
        {
            Update(model);
        }
    }
}
