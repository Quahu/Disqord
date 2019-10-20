using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestUser : RestSnowflakeEntity, IUser
    {
        public string Name { get; private set; }

        public string Discriminator { get; private set; }

        public string AvatarHash { get; private set; }

        public bool IsBot { get; }

        public string Tag => $"{Name}#{Discriminator}";

        public virtual string Mention => Discord.MentionUser(this);

        internal RestUser(RestDiscordClient client, UserModel model) : base(client, model.Id)
        {
            IsBot = model.Bot;
            Update(model);
        }

        internal virtual void Update(UserModel model)
        {
            Name = model.Username.Value;
            Discriminator = model.Discriminator.Value;
            AvatarHash = model.Avatar.Value;
        }

        public string GetAvatarUrl(ImageFormat format = default, int size = 2048)
            => Discord.Internal.GetAvatarUrl(this, format, size);

        public override string ToString()
            => Tag;
    }
}
