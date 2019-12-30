using Disqord.Models;

namespace Disqord.Rest
{
    public sealed class RestDmChannel : RestPrivateChannel, IDmChannel
    {
        public RestUser Recipient { get; }

        IUser IDmChannel.Recipient => Recipient;

        internal RestDmChannel(RestDiscordClient client, ChannelModel model) : base(client, model)
        {
            Recipient = new RestUser(Client, model.Recipients.Value[0]);
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            model.Name = Recipient.Tag;
            base.Update(model);
        }
    }
}
