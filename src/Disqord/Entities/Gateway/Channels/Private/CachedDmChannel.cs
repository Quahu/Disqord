using Disqord.Models;

namespace Disqord
{
    public sealed class CachedDmChannel : CachedPrivateChannel, IDmChannel
    {
        public CachedUser Recipient { get; }

        IUser IDmChannel.Recipient => Recipient;

        internal CachedDmChannel(DiscordClientBase client, ChannelModel model) : base(client, model)
        {
            Recipient = client.State.GetOrAddSharedUser(model.Recipients.Value[0]);
            Update(model);
        }

        internal override void Update(ChannelModel model)
        {
            model.Name = Recipient.Tag;

            base.Update(model);
        }
    }
}
