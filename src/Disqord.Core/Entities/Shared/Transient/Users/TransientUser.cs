using Disqord.Models;

namespace Disqord
{
    public class TransientUser : TransientEntity<UserJsonModel>, IUser
    {
        /// <inheritdoc/>
        public virtual Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public virtual string Name => Model.Username;

        /// <inheritdoc/>
        public virtual string Discriminator => Model.Discriminator.ToString("0000");

        /// <inheritdoc/>
        public virtual string AvatarHash => Model.Avatar;

        /// <inheritdoc/>
        public virtual bool IsBot => Model.Bot.GetValueOrDefault();

        /// <inheritdoc/>
        public virtual string BannerHash => Model.Banner.Value;

        /// <inheritdoc/>
        public virtual Color? AccentColor => Model.AccentColor.HasValue
            ? Model.AccentColor.Value
            : null;

        /// <inheritdoc/>
        public virtual UserFlag PublicFlags => Model.PublicFlags.Value;

        /// <inheritdoc/>
        public virtual string Mention => Disqord.Mention.User(this);

        /// <inheritdoc/>
        public virtual string Tag => $"{Name}#{Discriminator}";

        public TransientUser(IClient client, UserJsonModel model)
            : base(client, model)
        { }

        public override string ToString()
            => Tag;
    }
}
