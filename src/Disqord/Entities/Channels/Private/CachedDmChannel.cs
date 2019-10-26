using Disqord.Models;

namespace Disqord
{
    public sealed class CachedDmChannel : CachedPrivateChannel, IDmChannel
    {
        public CachedUser Recipient { get; }

        IUser IDmChannel.Recipient => Recipient;

        internal CachedDmChannel(DiscordClient client, ChannelModel model) : base(client, model)
        {
            Recipient = client.GetOrAddSharedUser(model.Recipients.Value[0]);
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            model.Name = Recipient.Tag;

            base.Update(model);
        }
    }
}
