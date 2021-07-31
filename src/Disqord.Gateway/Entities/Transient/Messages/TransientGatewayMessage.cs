using Disqord.Api;
using Disqord.Gateway;
using Disqord.Models;

namespace Disqord
{
    public abstract class TransientGatewayMessage : TransientMessage, IGatewayMessage
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

        protected TransientGatewayMessage(IClient client, MessageJsonModel model)
            : base(client, model)
        { }

        /// <summary>
        ///     Creates either a <see cref="TransientGatewayUserMessage"/> or a <see cref="TransientSystemMessage"/> based on the type.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static new IGatewayMessage Create(IClient client, MessageJsonModel model)
        {
            switch (model.Type)
            {
                case MessageType.Default:
                case MessageType.Reply:
                case MessageType.ApplicationCommand:
                case MessageType.ThreadStarterMessage:
                    return new TransientGatewayUserMessage(client, model);

                default:
                    return new TransientGatewaySystemMessage(client, model);
            }
        }
    }
}
