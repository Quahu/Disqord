using Disqord.Models;

namespace Disqord
{
    public class TransientDirectChannel : TransientPrivateChannel, IDirectChannel
    {
        public override string Name => Recipient.Tag;

        public IUser Recipient
        {
            get
            {
                if (_recipient == null)
                    _recipient = new TransientUser(Client, Model.Recipients.Value[0]);

                return _recipient;
            }
        }
        private IUser? _recipient;

        public TransientDirectChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
