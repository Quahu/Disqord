using System.Globalization;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    public class TransientCurrentUser : TransientUser, ICurrentUser
    {
        /// <inheritdoc/>
        public virtual CultureInfo Locale => Discord.Internal.GetLocale(Model.Locale.GetValueOrDefault());

        /// <inheritdoc/>
        public virtual bool IsVerified => Model.Verified.Value;

        /// <inheritdoc/>
        public virtual string Email => Model.Email.Value;

        /// <inheritdoc/>
        public virtual bool HasMfaEnabled => Model.MfaEnabled.Value;

        /// <inheritdoc/>
        public virtual UserFlag Flags => Model.Flags.Value;

        /// <inheritdoc/>
        public virtual NitroType? NitroType => Model.PremiumType.Value;

        public TransientCurrentUser(IClient client, UserJsonModel model)
            : base(client, model)
        { }
    }
}
