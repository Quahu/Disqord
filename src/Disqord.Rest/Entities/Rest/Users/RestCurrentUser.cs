using Disqord.Models;

namespace Disqord.Rest
{
    public sealed partial class RestCurrentUser : RestUser, ICurrentUser
    {
        public string Locale { get; private set; }

        public bool IsVerified { get; private set; }

        public string Email { get; private set; }

        public bool HasMfaEnabled { get; private set; }

        internal RestCurrentUser(RestDiscordClient client, UserModel model) : base(client, model)
        {
            Update(model);
        }

        internal override void Update(UserModel model)
        {
            if (model.Locale.HasValue)
                Locale = model.Locale.Value;

            if (model.Verified.HasValue)
                IsVerified = model.Verified.Value.GetValueOrDefault();

            if (model.Email.HasValue)
                Email = model.Email.Value;

            if (model.MfaEnabled.HasValue)
                HasMfaEnabled = model.MfaEnabled.Value.GetValueOrDefault();

            base.Update(model);
        }
    }
}