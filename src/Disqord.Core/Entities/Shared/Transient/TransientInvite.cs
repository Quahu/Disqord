using Disqord.Models;

namespace Disqord
{
    public class TransientInvite : TransientEntity<InviteJsonModel>, IInvite
    {
        public Snowflake ChannelId { get; }

        public string Code => Model.Code;

        public IInviteMetadata Metadata
        {
            get
            {
                if (_metadata == null && Model.Inviter.HasValue)
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
