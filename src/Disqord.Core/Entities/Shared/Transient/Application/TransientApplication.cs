using System.Collections.Generic;
using Disqord.Models;
using Qommon.Collections;

namespace Disqord
{
    /// <inheritdoc cref="IApplication"/>
    public class TransientApplication : TransientClientEntity<ApplicationJsonModel>, IApplication
    {
        /// <inheritdoc/>
        public Snowflake Id => Model.Id;

        /// <inheritdoc/>
        public string Name => Model.Name;

        /// <inheritdoc/>
        public string IconHash => Model.Icon;

        /// <inheritdoc/>
        public string Description => Model.Description;

        /// <inheritdoc/>
        public bool IsBotPublic => Model.BotPublic;

        /// <inheritdoc/>
        public bool BotRequiresCodeGrant => Model.BotRequireCodeGrant;

        /// <inheritdoc/>
        public string TermsOfServiceUrl => Model.TermsOfServiceUrl.GetValueOrDefault();

        /// <inheritdoc/>
        public string PrivacyPolicyUrl => Model.PrivacyPolicyUrl.GetValueOrDefault();

        /// <inheritdoc/>
        public IUser Owner
        {
            get
            {
                if (!Model.Owner.HasValue)
                    return null;

                return _owner ??= new TransientUser(Client, Model.Owner.Value);
            }
        }
        private TransientUser _owner;

        /// <inheritdoc/>
        public IApplicationTeam Team
        {
            get
            {
                if (_team == null && Model.Team != null)
                    _team = new TransientApplicationTeam(Client, Model.Team);

                return _team;
            }
        }
        private IApplicationTeam _team;

        /// <inheritdoc/>
        public Optional<ApplicationFlag> Flags => Model.Flags;

        /// <inheritdoc/>
        public IReadOnlyList<string> Tags
        {
            get
            {
                if (!Model.Tags.HasValue)
                    return ReadOnlyList<string>.Empty;

                return Model.Tags.Value.ToReadOnlyList();
            }
        }

        /// <inheritdoc/>
        public IApplicationInstallationParameters InstallationParameters
        {
            get
            {
                if (!Model.InstallParams.HasValue)
                    return null;

                return _installationParameters ??= new TransientApplicationInstallationParameters(Model.InstallParams.Value);
            }
        }
        private IApplicationInstallationParameters _installationParameters;

        /// <inheritdoc/>
        public string AuthorizationUrl => Model.CustomInstallUrl.GetValueOrDefault();

        public TransientApplication(IClient client, ApplicationJsonModel model)
            : base(client, model)
        { }

        public static TransientApplication Create(IClient client, ApplicationJsonModel model)
        {
            if (model.Slug.HasValue)
                return new TransientStoreApplication(client, model);

            return new TransientApplication(client, model);
        }
    }
}
