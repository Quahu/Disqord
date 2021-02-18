using System.Globalization;
using Disqord.Models;

namespace Disqord
{
    public class TransientCurrentUser : TransientUser, ICurrentUser
    {
        public virtual CultureInfo Locale => Discord.Internal.GetLocale(Model.Locale.GetValueOrDefault());

        public virtual bool IsVerified => Model.Verified.Value;

        public virtual string Email => Model.Email.Value;

        public virtual bool HasMfaEnabled => Model.MfaEnabled.Value;

        public virtual UserFlag Flags => Model.Flags.Value;

        public virtual NitroType? NitroType => Model.PremiumType.Value;

        public TransientCurrentUser(IClient client, UserJsonModel model)
            : base(client, model)
        { }
    }
}
