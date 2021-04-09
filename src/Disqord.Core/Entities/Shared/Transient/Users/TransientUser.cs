using System;
using Disqord.Models;

namespace Disqord
{
    public class TransientUser : TransientEntity<UserJsonModel>, IUser
    {
        public virtual Snowflake Id => Model.Id;

        public virtual DateTimeOffset CreatedAt => Id.CreatedAt;

        public virtual string Name => Model.Username;

        public virtual string Discriminator => Model.Discriminator;

        public virtual string AvatarHash => Model.Avatar;

        public virtual bool IsBot => Model.Bot.GetValueOrDefault();

        public virtual UserFlag PublicFlags => Model.PublicFlags.Value;

        public virtual string Mention => Disqord.Mention.User(this);

        public virtual string Tag => $"{Name}#{Discriminator}";

        public TransientUser(IClient client, UserJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => Tag;
    }
}
