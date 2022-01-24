using Disqord.Models;

namespace Disqord
{
    public class TransientGuildInvite : TransientInvite, IGuildInvite
    {
        /// <inheritdoc/>
        public Snowflake GuildId => Model.Guild.Value.Id;

        /// <inheritdoc/>
        public IInviteGuild Guild => _guild ??= new TransientInviteGuild(Client, Model.Guild.Value);
        private IInviteGuild _guild;

        /// <inheritdoc/>
        public InviteTargetType? TargetType => Model.TargetType.GetValueOrNullable();

        /// <inheritdoc/>
        public IUser TargetUser
        {
            get
            {
                if (!Model.TargetUser.HasValue)
                    return null;

                return _targetUser ??= new TransientUser(Client, Model.TargetUser.Value);
            }
        }
        private IUser _targetUser;

        /// <inheritdoc/>
        public IApplication TargetApplication
        {
            get
            {
                if (!Model.TargetApplication.HasValue)
                    return null;

                return _targetApplication ??= TransientApplication.Create(Client, Model.TargetApplication.Value);
            }
        }
        private IApplication _targetApplication;

        /// <inheritdoc/>
        public IInviteStage Stage
        {
            get
            {
                if (!Model.StageInstance.HasValue)
                    return null;

                return _stage ??= new TransientInviteStage(Client, GuildId, Model.StageInstance.Value);
            }
        }
        private IInviteStage _stage;

        /// <inheritdoc/>
        public IGuildEvent Event
        {
            get
            {
                if (!Model.GuildScheduledEvent.HasValue)
                    return null;

                return _guildEvent ??= new TransientGuildEvent(Client, Model.GuildScheduledEvent.Value);
            }
        }
        private IGuildEvent _guildEvent;

        /// <inheritdoc/>
        public int? ApproximatePresenceCount => Model.ApproximatePresenceCount.GetValueOrNullable();

        /// <inheritdoc/>
        public IInviteMetadata Metadata
        {
            get
            {
                if (_metadata == null && Model.Uses.HasValue)
                    _metadata = new TransientInviteMetadata(Model);

                return _metadata;
            }
        }
        private IInviteMetadata _metadata;

        public TransientGuildInvite(IClient client, InviteJsonModel model)
            : base(client, model)
        { }
    }
}
