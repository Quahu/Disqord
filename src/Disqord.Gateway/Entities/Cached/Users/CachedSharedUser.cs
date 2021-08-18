﻿using System.ComponentModel;
using Disqord.Models;

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
        public override string BannerHash => _bannerHash;

        /// <inheritdoc/>
        public override Color? AccentColor => _accentColor;

        /// <inheritdoc/>
        public override UserFlag PublicFlags => _publicFlags;

        /// <inheritdoc/>
        public int ReferenceCount
        {
            get
            {
                lock (this)
                {
                    return _referenceCount;
                }
            }
        }

        private string _name;
        private short _discriminator;
        private string _avatarHash;
        private readonly bool _isBot;
        private string _bannerHash;
        private Color? _accentColor;
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

            if (model.Banner.HasValue)
                _bannerHash = model.Banner.Value;

            if (model.AccentColor.HasValue)
                _accentColor = model.AccentColor.Value;

            if (model.PublicFlags.HasValue)
                _publicFlags = model.PublicFlags.Value;
        }

        /// <inheritdoc/>
        public int AddReference(CachedUser user)
        {
            lock (this)
            {
                return ++_referenceCount;
            }
        }

        /// <inheritdoc/>
        public int RemoveReference(CachedUser user)
        {
            lock (this)
            {
                return --_referenceCount;
            }
        }
    }
}
