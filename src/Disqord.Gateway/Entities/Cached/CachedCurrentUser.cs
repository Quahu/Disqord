using System.Collections.Generic;
using System.ComponentModel;
using Disqord.Collections.Synchronized;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedSharedUser : CachedUser, ICachedSharedUser
    {
        /// <inheritdoc/>
        public override string Name => _name;

        /// <inheritdoc/>
        public override string Discriminator => _discriminator;

        /// <inheritdoc/>
        public override string AvatarHash => _avatarHash;

        /// <inheritdoc/>
        public override bool IsBot => _isBot;

        /// <inheritdoc/>
        public ISet<CachedUser> References => _references;

        private string _name;
        private string _discriminator;
        private string _avatarHash;
        private readonly bool _isBot;

        private readonly SynchronizedHashSet<CachedUser> _references;

        /// <summary>
        ///     Instantiates a new shared user.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="model"></param>
        public CachedSharedUser(IGatewayClient client, UserJsonModel model)
            : base(client, model)
        {
            _isBot = model.Bot.GetValueOrDefault();

            _references = new SynchronizedHashSet<CachedUser>();

            Update(model);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(UserJsonModel model)
        {
            _name = model.Username;
            _discriminator = model.Discriminator;
            _avatarHash = model.Avatar;
        }
    }
}
