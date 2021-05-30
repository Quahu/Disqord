using Disqord.Models;

namespace Disqord
{
    /// <inheritdoc cref="IApplication"/>
    public class TransientApplication : TransientEntity<ApplicationJsonModel>, IApplication
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
        public IUser Owner => _owner ??= new TransientUser(Client, Model.Owner);
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
        public Optional<int> Flags => Model.Flags;

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
