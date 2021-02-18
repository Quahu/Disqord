using System.Globalization;
using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedCurrentUser : CachedUser, ICurrentUser
    {
        public CultureInfo Locale { get; private set; }

        public bool IsVerified { get; private set; }

        public string Email { get; private set; }

        public bool HasMfaEnabled { get; private set; }

        public UserFlag Flags { get; private set; }

        public bool HasNitro => NitroType != null;

        public NitroType? NitroType { get; private set; }

        public CachedCurrentUser(ICachedSharedUser sharedUser, UserJsonModel model)
            : base(sharedUser)
        {
            Update(model);
        }

        public override void Update(UserJsonModel model)
        {
            if (model.Locale.HasValue)
                Locale = CultureInfo.ReadOnly(new CultureInfo(model.Locale.Value ?? "en-US"));

            if (model.Verified.HasValue)
                IsVerified = model.Verified.Value;

            if (model.Email.HasValue)
                Email = model.Email.Value;

            if (model.MfaEnabled.HasValue)
                HasMfaEnabled = model.MfaEnabled.Value;

            if (model.Flags.HasValue)
                Flags = (UserFlag) model.Flags.Value;

            if (model.PremiumType.HasValue)
                NitroType = (NitroType) model.PremiumType.Value;

            base.Update(model);
        }
    }
}
