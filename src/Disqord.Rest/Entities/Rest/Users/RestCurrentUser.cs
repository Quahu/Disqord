using System.Globalization;
using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestCurrentUser : RestUser, ICurrentUser
    {
        public CultureInfo Locale { get; private set; }

        public bool IsVerified { get; private set; }

        public string Email { get; private set; }

        public bool HasMfaEnabled { get; private set; }

        public string Phone { get; private set; }

        public UserFlags Flags { get; private set; }

        public bool HasNitro { get; private set; }

        public NitroType? NitroType { get; private set; }

        internal RestCurrentUser(RestDiscordClient client, UserModel model) : base(client, model)
        {
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

            base.Update(model);
        }
    }
}