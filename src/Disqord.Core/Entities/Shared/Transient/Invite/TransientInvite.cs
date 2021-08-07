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
        public IInviteChannel Channel => _channel ??= new TransientInviteChannel(Client, Model.Channel);
        private IInviteChannel _channel;

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

        public TransientInvite(IClient client, InviteJsonModel model)
            : base(client, model)
        { }

        public static TransientInvite Create(IClient client, InviteJsonModel model)
            => model.Guild.HasValue ? new TransientGuildInvite(client, model) : new TransientInvite(client, model);

    }
}
