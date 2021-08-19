using System;
using System.ComponentModel;
using Disqord.Models;

namespace Disqord.Gateway
{
    public abstract class CachedUser : CachedSnowflakeEntity, IUser
    {
        public virtual string Name => SharedUser.Name;

        public virtual string Discriminator => SharedUser.Discriminator;

        public virtual string AvatarHash => SharedUser.AvatarHash;

        public virtual bool IsBot => SharedUser.IsBot;

        public virtual string BannerHash => SharedUser.BannerHash;

        public virtual Color? AccentColor => SharedUser.AccentColor;

        public virtual UserFlag PublicFlags => SharedUser.PublicFlags;

        public string Mention => Disqord.Mention.User(this);

        public string Tag => $"{Name}#{Discriminator}";

        [EditorBrowsable(EditorBrowsableState.Never)]
        public CachedSharedUser SharedUser
        {
            get
            {
                if (_sharedUser == null)
                {
                    if (this is CachedSharedUser sharedUser)
                        return sharedUser;

                    throw new InvalidOperationException("This user has no shared user attached to it.");
                }

                return _sharedUser;
            }
            internal set => _sharedUser = value;
        }
        private CachedSharedUser _sharedUser;

        /// <summary>
        ///     Instantiates a new user from the provided <see cref="ICachedSharedUser"/>.
        /// </summary>
        /// <param name="sharedUser"></param>
        protected CachedUser(CachedSharedUser sharedUser)
            : base(sharedUser.Client, sharedUser.Id)
        {
            _sharedUser = sharedUser;
            _sharedUser.AddReference(this);
        }

        /// <summary>
        ///     Instantiates a new user from the provided client and model.
        ///     This constructor should be used exclusively by <see cref="ICachedSharedUser"/> implementations.
        /// </summary>
        /// <param name="client"> The client that created this user. </param>
        /// <param name="model"> The model to create the user from. </param>
        protected CachedUser(IGatewayClient client, UserJsonModel model)
            : base(client, model.Id)
        { }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Update(UserJsonModel model)
            => SharedUser.Update(model);

        public override string ToString()
            => Tag;
    }
}
