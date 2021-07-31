using Disqord.Api;
using Disqord.Gateway;
using Disqord.Models;

namespace Disqord
{
    public class TransientGatewayUserMessage : TransientUserMessage, IGatewayUserMessage
    {
        public Snowflake? GuildId => Model.GuildId.GetValueOrNullable();

        public override IUser Author
        {
            get
            {
                if (!Model.GuildId.HasValue || !Model.Member.HasValue)
                    return base.Author;

                if (_author == null)
                {
                    // Following trick lets us not duplicate logic.
                    Model.Member.Value.User = Model.Author;
                    _author = new TransientMember(Client, GuildId.Value, Model.Member.Value);
                }

                return _author as IMember;
            }
        }

        public override Optional<IUserMessage> ReferencedMessage
        {
            get
            {
                if (Model.Type == MessageType.Reply || Model.Type == MessageType.ThreadStarterMessage || Model.ReferencedMessage.GetValueOrDefault() != null)
                {
                    // Fix for Discord always sending an empty property.
                    return base.ReferencedMessage;
                }

                return default;
            }
        }

        public TransientGatewayUserMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }
    }
}
