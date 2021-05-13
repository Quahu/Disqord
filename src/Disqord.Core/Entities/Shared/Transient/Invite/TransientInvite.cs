using Disqord.Models;

namespace Disqord
{
    public class TransientInvite : TransientEntity<InviteJsonModel>, IInvite
    {
        /// <inheritdoc/>
        public Snowflake ChannelId => Model.Channel.Id;

        /// <inheritdoc/>
        public string Code => Model.Code;

        /// <inheritdoc/>
        public IUser Inviter
        {
            get
            {
                if (!Model.Inviter.HasValue)
                    return null;

                return _inviter ??= new TransientUser(Client, Model.Inviter.Value);
            }
        }
        private IUser _inviter;

        /// <inheritdoc/>
        public IInviteMetadata Metadata
        {
            get
            {
                if (_metadata == null && Model.Uses.HasValue)
                    _metadata = new TransientInviteMetadata(Client, Model);

                return _metadata;
            }
        }
        private IInviteMetadata _metadata;

        public TransientInvite(IClient client, InviteJsonModel model)
            : base(client, model)
        { }
    }
}
