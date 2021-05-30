using Disqord.Models;

namespace Disqord
{
    public class TransientDirectChannel : TransientPrivateChannel, IDirectChannel
    {
        public override string Name => Recipient.Tag;

        public IUser Recipient => _recipient ??= new TransientUser(Client, Model.Recipients.Value[0]);
        private IUser _recipient;

        public TransientDirectChannel(IClient client, ChannelJsonModel model)
            : base(client, model)
        { }
    }
}
