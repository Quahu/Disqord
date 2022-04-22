using System.ComponentModel;
using System.Threading;
using Disqord.Models;
using Qommon;

namespace Disqord.Gateway
{
    public class CachedSharedUser : CachedUser, ICachedSharedUser
    {
        /// <inheritdoc/>
        public override string Name => _name;

        /// <inheritdoc/>
        public override string Discriminator => _discriminator.ToString("0000");

        /// <inheritdoc/>
        public override string AvatarHash => _avatarHash;

        /// <inheritdoc/>
        public override bool IsBot => _isBot;

        /// <inheritdoc/>
        public override UserFlag PublicFlags => _publicFlags;

        /// <inheritdoc/>
        public int ReferenceCount => _referenceCount;

        private string _name;
        private short _discriminator;
        private string _avatarHash;
        private readonly bool _isBot;
        private UserFlag _publicFlags;
        private int _referenceCount;

        /// <summary>
        ///     Instantiates a new shared user.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="model"></param>
        public CachedSharedUser(IGatewayClient client, UserJsonModel model)
            : base(client, model)
        {
            _isBot = model.Bot.GetValueOrDefault();

            Update(model);
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void Update(UserJsonModel model)
        {
            _name = model.Username;
            _discriminator = model.Discriminator;
            _avatarHash = model.Avatar;

            if (model.PublicFlags.HasValue)
                _publicFlags = model.PublicFlags.Value;
        }

        /// <inheritdoc/>
        public int AddReference(CachedUser user)
            => Interlocked.Increment(ref _referenceCount);

        /// <inheritdoc/>
        public int RemoveReference(CachedUser user)
            => Interlocked.Decrement(ref _referenceCount);
    }
}
