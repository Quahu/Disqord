using Disqord.Models;

namespace Disqord
{
    public sealed class CachedUnknownUser : CachedUser
    {
        public override string Name { get; protected set; }

        public override string Discriminator { get; protected set; }

        public override string AvatarHash { get; protected set; }

        public override UserStatus Status { get; }

        public override Activity Activity { get; }

        internal override CachedSharedUser SharedUser { get; }

        internal CachedUnknownUser(DiscordClient client, UserModel model) : base(client, model.Id)
        {
            Name = model.Username.Value;
            Discriminator = model.Discriminator.Value;
            AvatarHash = model.Avatar.Value;
        }

        internal override void Update(UserModel model)
        { }
    }
}
