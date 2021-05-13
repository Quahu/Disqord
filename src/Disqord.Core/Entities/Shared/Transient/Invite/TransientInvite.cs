using Disqord.Models;

namespace Disqord
{
    public class TransientInvite : TransientEntity<InviteJsonModel>, IInvite
    {
        public Snowflake ChannelId => Model.Channel.Id;

        public string Code => Model.Code;

        public Optional<IUser> Inviter
        {
            get
            {
                if (!Model.Inviter.HasValue)
                    return default;

                if (_inviter == null)
                    _inviter = Optional.Convert(Model.Inviter, x => new TransientUser(Client, x) as IUser);

                return _inviter.Value;
            }
        }
        private Optional<IUser>? _inviter;

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
