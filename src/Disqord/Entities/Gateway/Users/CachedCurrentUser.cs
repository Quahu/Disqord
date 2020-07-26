using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord
{
    public sealed partial class CachedCurrentUser : CachedUser, ICurrentUser
    {
        public override string Name => SharedUser.Name;

        public override string Discriminator => SharedUser.Discriminator;

        public override string AvatarHash => SharedUser.AvatarHash;

        public override bool IsBot => SharedUser.IsBot;

        public CultureInfo Locale { get; private set; }

        public bool IsVerified { get; private set; }

        public string Email { get; private set; }

        public bool HasMfaEnabled { get; private set; }

        public string Phone { get; private set; }

        public UserFlags Flags { get; private set; }

        public bool HasNitro { get; private set; }

        public NitroType? NitroType { get; private set; }

        /// <summary>
        ///     Throws <see cref="InvalidOperationException"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override CachedDmChannel DmChannel => throw new InvalidOperationException();

        internal override CachedSharedUser SharedUser { get; }

        internal CachedCurrentUser(CachedSharedUser user, UserModel model) : base(user)
        {
            SharedUser = user;

            Update(model);
        }

        internal override void Update(UserModel model)
        {
            if (model.Locale.HasValue)
                Locale = Discord.Internal.CreateLocale(model.Locale.Value);

            if (model.Verified.HasValue)
                IsVerified = model.Verified.Value;

            if (model.Email.HasValue)
                Email = model.Email.Value;

            if (model.MfaEnabled.HasValue)
                HasMfaEnabled = model.MfaEnabled.Value;

            if (model.Phone.HasValue)
                Phone = model.Phone.Value;

            if (model.Flags.HasValue)
                Flags = model.Flags.Value;

            if (model.Premium.HasValue)
                HasNitro = model.Premium.Value;

            if (model.PremiumType.HasValue)
                NitroType = model.PremiumType.Value;

            SharedUser.Update(model);
        }
    }
}